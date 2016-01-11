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

    class Lokun
    {
        // singleton
        public static Lokun instance = new Lokun();

        private ServiceController _openvpn_service = new ServiceController("OpenVPNService");
        private string _path = ".";

        public bool CheckConnectedAsync()
        {
            // BAD: Phoning home.
            var client = new RestClient("https://lokun.is/");
            var request = new RestRequest("connected.json");
            request.AddHeader("User-Agent", "lokun-client0.2");
            bool connected = false;
            client.ExecuteAsync<Connected>(request, response =>
            {
                if (response.Data == null)
                {
                    connected = false;
                }
                else
                {
                    connected = response.Data.connected;
                }
            });
            return connected;
        }

        public ConnectionStatus FullyCheckConnectionStatusAsync()
        {
            // This is a method and not a property because it has side-effects.
            // It's named Async because it calls an Async method.
            if (!OpenVPNServiceIsInstalled)
            {
                return ConnectionStatus.OpenVPNNotInstalled;
            }
            else if (CheckConnectedAsync())
            {
                // Is traffic to lokun.is routed through Lokun?
                return ConnectionStatus.EverythingTunneled;
            }
            else if (PingVPNNode())
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

        public void ToogleStartStop()
        {
            // Make async is blocks
            _openvpn_service.Refresh();
            if (_openvpn_service.Status != ServiceControllerStatus.Running)
            {
                _openvpn_service.Start();
            }
            else
            {
                _openvpn_service.Stop();
            }
        }

        public string GetCurrentUsername()
        {
            return Directory.GetFiles(_path, "*.key")
                .Select(Path.GetFileNameWithoutExtension)
                .FirstOrDefault();
        }

        public void DownloadConfigAsync(string username, string password)
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

                    var response = client.PostAsync(url, values_encoded);
                    if (response.Result.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Result.Content.ReadAsByteArrayAsync();

                        using (var ms = new MemoryStream(content.Result))
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
                        var error_content = response.Result.Content.ReadAsStringAsync();
                        var apierror = JsonConvert.DeserializeObject<APIError>(error_content.Result);
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

        public bool PingVPNNode()
        {
            // Side-effects -> method
            //
            // BAD: An upstream network provider will recieve the ping packet and can
            // fingerprint/note users when not connected
            var pinger = new Ping();
            try
            {
                return pinger.Send("10.40.20.1").Status == IPStatus.Success;
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
    }
}
