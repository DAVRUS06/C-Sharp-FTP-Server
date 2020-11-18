using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


/// <summary>
/// Code for handling the UI of the application. 
/// </summary>

namespace FTPserver
{
    public partial class mainForm : Form
    {
        server ftpServer = new server();
        
        public mainForm()
        {
            InitializeComponent();
        }

        // The startServer function starts the server and begins listening for clients that are trying to connect
        private void startServer_Click(object sender, EventArgs e)
        {
            // Get the information from the text boxes
            string path = sharedFoldertxtbox.Text;
            int port;
            Int32.TryParse(porttxtbox.Text, out port);
            string user = userNametxtbox.Text;
            string pass = passwordtxtbox.Text;

            // Set the status bar to indication server is running
            serverStatus.Text = "Server Status: Running...";
            serverStatus.BackColor = Color.Green;

            // Disable the start server button and enable the stop server button
            stopListeningbtn.Enabled = true;
            startServerbtn.Enabled = false;

            // Start the server
            ftpServer.start(user, pass, path, port);
        }

        // The openSharedFolder funciton is how the user selects which folder to share with the clients
        private void openSharedFolderbtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select the folder to be shared." })
            {
                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    sharedFoldertxtbox.Text = fbd.SelectedPath;
                }
            }
            startServerbtn.Enabled = true;
        }

        // The stopListeningbtn function stops the server from listening for more clients.
        private void stopListeningbtn_Click(object sender, EventArgs e)
        {
            // Stop the server from listening
            ftpServer.stopListening();
            // Enable the start server button
            startServerbtn.Enabled = true;
            // Disable the stop server button
            stopListeningbtn.Enabled = false;

            // Update the status bar to indicate the server is not listening
            serverStatus.Text = "Server Status: Not Running.";
            serverStatus.BackColor = Color.Red;
        }

    }

}
