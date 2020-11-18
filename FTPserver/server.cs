using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

/// <summary>
/// Class for setting up the the server for listening and then handling the clients on their own thread.
/// </summary>

namespace FTPserver
{
    class server
    {
        private TcpListener listenForClients;
        
        private string username;
        private string password;
        private string rootFolder;
        private int PORT;
        private bool listeningForClients;
        private bool disconnecting;
        private List<connection> connectionList = new List<connection>();

        public server()
        {
            // Default constructor
        }


        //    The start function is what begins to operate the server.
        //    The user/pass strings are the credentials for the client to user to connect with
        //    The root string is the path of the folder which is to be shared with the clients
        //    The portNum is the port which the server will operate from
        public void start(string user, string pass, string root, int portNum)
        {
            username = user;
            password = pass;
            rootFolder = root;
            PORT = portNum;
            listeningForClients = true;
            listenForClients = new TcpListener(IPAddress.Any, PORT);
            listenForClients.Start();
            listenForClients.BeginAcceptTcpClient(handleClient, listenForClients);

        }

        // Handle the client
        // Setup the thread that will handle the client when connected
        private void handleClient(IAsyncResult result)
        {
            if(listeningForClients)
            {
                TcpClient client = listenForClients.EndAcceptTcpClient(result);
                listenForClients.BeginAcceptTcpClient(handleClient, listenForClients);

                connection clientCon = new connection(client, username, password, rootFolder);
                connectionList.Add(clientCon);
                ThreadPool.QueueUserWorkItem(clientCon.handleClient, client);
            }
        }

        // Server stops listening for more clients
        // The stopListening function is what will stop the server from listening for more clients. 
        public void stopListening()
        {
                listeningForClients = false;
                listenForClients.Stop();
                listenForClients = null;
        }

    }
}
