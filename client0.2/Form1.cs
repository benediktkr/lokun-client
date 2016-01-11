using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RestSharp;

using lokunclient.Models;

namespace lokunclient
{
    public partial class Form1 : Form
    {
        private Lokun lokun;
        private ConnectionStatus connected;

        public Form1()
        {
            InitializeComponent();
            lokun = Lokun.instance;
            connected = ConnectionStatus.NotRunning;
            chkAutostart.Checked = lokun.Autostart;
            var username = lokun.GetCurrentUsername();
            if (username != null)
            {
                txtUsername.Text = username;
                txtPassword.Text = "hunter2";
                ststrpLabel.Text = "Current account: " + username;
            }
            btnStop.Text = lokun.OpenVPNServiceIsRunning ? "Stop" : "Start";
        }

        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            var lokun = Lokun.instance;
            lblCheckConnection.Text = "Checking...";
            lblCheckConnection.Refresh();
            connected = lokun.FullyCheckConnectionStatusAsync();
            if (connected == ConnectionStatus.EverythingTunneled)
            {
                lblCheckConnection.Text = "Tunnel active";
            }
            else if (connected == ConnectionStatus.ConnectedWithoutDGW)
            {
                lblCheckConnection.Text = "Tunnel idle";
            }
            else if (connected == ConnectionStatus.RunningNotConnected)
            {
                lblCheckConnection.Text = "Tunnel not in use";
            }
            else if (connected == ConnectionStatus.OpenVPNNotInstalled)
            {
                lblCheckConnection.Text = "Error: OpenVPN missing";
            }
            else
            {
                lblCheckConnection.Text = "Tunnel down";
            }
            lblCheckConnection.Refresh();
        }

        private void chkAutostart_CheckedChanged(object sender, EventArgs e)
        {
            lokun.SetAutostart(chkAutostart.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ststrpLabel.Text = "Downloading your configuration..";
                lokun.DownloadConfigAsync(txtUsername.Text, txtPassword.Text);
                ststrpLabel.Text = "Downloaded Lokun configuration.";
            }
            catch (ApplicationException ex)
            {
                ststrpLabel.Text = ex.Message;
            }
        }


    }
}
