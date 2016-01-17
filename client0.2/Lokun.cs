using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;

using RestSharp;
using Newtonsoft.Json;

using lokunclient.Models;

namespace lokunclient
{
    public enum ConnectionStatus
    {
        NotRunning,
        RunningNotConnected,
        ConnectedToIS,      // NOT USED YET
        ConnectedBypassIS,  // NOT USED YET
        ConnectedWithoutDGW,
        EverythingTunneled,
        OpenVPNNotInstalled
    };

    public enum RoutingSetting
    {
        Everything,
        OnlyISNets,
        ExcludeISNets
    }

    class Lokun
    {
        // singleton
        public static Lokun instance = new Lokun();

        private ServiceController _openvpn_service = new ServiceController("OpenVPNService");
        private string _path = ".";
        private string _isnets_file = "http://www.rix.is/is-net.txt";
        private RoutingSetting _default_routing_setting = RoutingSetting.OnlyISNets;

        public async Task<bool> CheckConnectedAsync()
        {
            // BAD: Phoning home.
            var client = new RestClient("https://lokun.is/");
            var request = new RestRequest("connected.json");
            request.AddHeader("User-Agent", "lokun-client0.2");
            
            var response = await client.ExecuteGetTaskAsync<Connected>(request);

            if (response.Data == null)
            {
                return false;
            }
            else
            {
                return response.Data.connected;
            }
        }

        public async Task<ConnectionStatus> FullyCheckConnectionStatusAsync()
        {
            // This is a method and not a property because it has side-effects.
            // It's named Async because it calls an Async method.
            if (!OpenVPNServiceIsInstalled)
            {
                return ConnectionStatus.OpenVPNNotInstalled;
            }
            else if (await CheckConnectedAsync())
            {
                // Is traffic to lokun.is routed through Lokun?
                return ConnectionStatus.EverythingTunneled;
            }
            else if (await PingVPNNode())
            {
                // Can we talk to a VPN node?
                return ConnectionStatus.ConnectedWithoutDGW;
            }
            else if (OpenVPNServiceIsRunning)
            {
                // Is the service running?
                return ConnectionStatus.RunningNotConnected;
            }
            else
            {
                return ConnectionStatus.NotRunning;
            }
        }

        public async Task<bool> ToogleStartStopAsync()
        {
            try
            {
                _openvpn_service.Refresh();
                if (_openvpn_service.Status != ServiceControllerStatus.Running)
                {
                    await Task.Run(() => _openvpn_service.Start());
                }
                else
                {
                    await Task.Run(() => _openvpn_service.Stop());
                }
                _openvpn_service.Refresh();
                return _openvpn_service.Status == ServiceControllerStatus.Running;
            }
            catch (InvalidOperationException e)
            {
                throw new ApplicationException("OpenVPN is not installed", e);
            }
        }

        public string GetCurrentUsername()
        {
            return Directory.GetFiles(_path, "*.key")
                .Select(Path.GetFileNameWithoutExtension)
                .FirstOrDefault();
        }

        private Tuple<string, string> CidrToBitmask(string cidr_net)
        {
            // Pretty much in verbatim from Kalli's old code
            string[] net = cidr_net.Split('/');
            int cidr = int.Parse(net[1]);
            uint bitmask = 0xFFFFFFFF << (32 - cidr);
            byte[] bytes = BitConverter.GetBytes(bitmask);
            Array.Reverse(bytes);
            string netmask = bytes[0].ToString() + "." + bytes[1].ToString() + "." +
                             bytes[2].ToString() + "." + bytes[3].ToString();
            return new Tuple<string, string>(net[0], netmask);
        }

        private void AddRoute(string net, string netmask, string dgw)
        {
            // From Kalli's old code
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "route";
            p.StartInfo.Arguments = "add " + net + " mask " + netmask + " " + dgw;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }

        private void DelRoute(string net, string netmask)
        {
            // From Kalli's old code
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "route";
            p.StartInfo.Arguments = "delete " + net + " mask " + netmask;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }

        private string FindDefaultGW()
        {
            // From Kalli's old code
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "route";
            p.StartInfo.Arguments = "print";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.StandardOutputEncoding = Encoding.ASCII;
            p.Start();
            StreamReader output = p.StandardOutput;
            while (!output.EndOfStream)
            {
                string line = output.ReadLine();
                Match match = Regex.Match(line, @"0\.0\.0\.0.*0\.0\.0\.0");
                if (match.Success)
                {
                    string gw = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)[2];
                    return gw;
                }
            }
            // TODO: error handling
            return null;
        }
        
        public async Task<IEnumerable<Tuple<string, string>>> GetISNetsAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(_isnets_file);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var isnets = await response.Content.ReadAsStringAsync();
                        return isnets.Split('\n').Select(CidrToBitmask);
                    }
                    else
                    {
                        throw new ApplicationException("is-nets error");
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    // malformed file
                    throw new ApplicationException("is-nets error", ex);
                }
                catch (AggregateException ex)
                {
                    throw new ApplicationException("is-nets error", ex);
                }
            }
        }

        public async void DownloadConfigAsync(string username, string password)
        {
            if (!username.All(char.IsLetterOrDigit))
            {
                throw new ApplicationException("Invalid username");
            }

            using (var client = new HttpClient())
            {
                try
                {
                    var url = String.Format("https://lokun.is/api/users/{0}/config.zip", username);
                    var values = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("password", password)
                    };
                    var values_encoded = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync(url, values_encoded);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();

                        using (var ms = new MemoryStream(content))
                        {
                            using (var zip = new ZipArchive(ms))
                            {
                                foreach (var file in zip.Entries)
                                {
                                    // Overwrites existing files. (ca.crt is included in all config.zip's and
                                    // it's a good way to fix broken files. 
                                    file.ExtractToFile(file.FullName, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        var error_content = await response.Content.ReadAsStringAsync();
                        var apierror = JsonConvert.DeserializeObject<APIError>(error_content);
                        throw new ApplicationException(apierror.error);
                    }
                }
                catch (AggregateException e)
                {
                    throw new ApplicationException("Unable to download config", e);
                }
            }
        }

        public void SetAutostart(bool autostart)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (autostart)
            {
                rk.SetValue("Lokun", System.Windows.Forms.Application.ExecutablePath);
            }
            else
            {
                rk.DeleteValue("Lokun", false);
            }
        }

        public bool Autostart
        {
            get
            {
                var rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
                if (rk.GetValue("Lokun") == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public async Task<bool> PingVPNNode()
        {
            // Side-effects -> method
            //
            // BAD: An upstream network provider will recieve the ping packet and can
            // fingerprint/note users when not connected
            try
            {
                var pinger = new Ping();
                var pingresult = await pinger.SendPingAsync("10.40.20.1", 700);
                return pingresult.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }

        public bool OpenVPNServiceIsInstalled
        {
            get
            {
                // The other approach to this is EAFP because ServiceController will throw
                // a InvalidOperationException on .Start() if the service isn't installed. 
                // https://msdn.microsoft.com/en-us/library/yb9w7ytd(v=vs.110).aspx

                var ovpn = ServiceController.GetServices()
                    .FirstOrDefault(s => s.ServiceName == "OpenVPNService");
                return ovpn != null;
            }
        }

        public bool OpenVPNServiceIsRunning
        {
            get
            {
                if (!OpenVPNServiceIsInstalled)
                {
                    return false;
                }
                _openvpn_service.Refresh();
                return _openvpn_service.Status == ServiceControllerStatus.Running;
            }
        }

        public RoutingSetting UserRoutingSetting
        {
            get
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.routing))
                {
                    return _default_routing_setting;
                }
                return (RoutingSetting)Enum.Parse(typeof(RoutingSetting), Properties.Settings.Default.routing);
            }
            set
            {
                Properties.Settings.Default.routing = Enum.GetName(typeof(RoutingSetting), value);
            }
        }
    }
}
