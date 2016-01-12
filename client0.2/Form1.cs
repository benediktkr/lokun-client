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
    public partial class lokunclientform : Form
    {
        private Lokun lokun;
        private ConnectionStatus connected;

        public lokunclientform()
        {
            InitializeComponent();
            lokun = Lokun.instance;
            connected = ConnectionStatus.NotRunning;
            chkAutostart.Checked = lokun.Autostart;
            btnDownload.Enabled = false;
            ststrpLabel.Text = lokun.OpenVPNServiceIsRunning ? "Service is up" : "Service is down";

            var username = lokun.GetCurrentUsername();
            if (username != null)
            {
                txtUsername.Text = username;
                txtPassword.Text = "hunter2";
                //ststrpLabel.Text = "Current account: " + username;
            }
        }

        private async void btnCheckConnection_Click(object sender, EventArgs e)
        {
            var lokun = Lokun.instance;
            lblCheckConnection.Text = "Checking...";
            lblCheckConnection.Refresh();
            connected = await lokun.FullyCheckConnectionStatusAsync();
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

        private void btnDownload_Click(object sender, EventArgs e)
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

        private void txtPassword_Enter(object sender, EventArgs e)
        {

        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            btnDownload.Enabled = true;
        }

    }
}
