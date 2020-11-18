using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

/// <summary>
/// Main class for handling the connection and all operations between the server and clients
/// </summary>

namespace FTPserver
{

    class connection
    {
        private TcpClient client;
        private TcpClient dataTrasnferClient;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private StreamReader readData;
        private StreamWriter writeData;

        private string username;
        private string serverUsername;
        private string password;
        private string serverPassword;
        private string root;
        private string userRoot;
        private string currentDirectory;
        private string transferTypeCode = "I";
        private string transferMode = "Passive";
        private string renaming;

        private IPEndPoint dataEndpoint;
        private IPEndPoint remoteEndpoint;
        private TcpListener passiveListen;
        private Boolean hadFailure;


        // The constructor for the connection class, sets up the object for connection with the client requesting access
        public connection(TcpClient newClient, string serverUser, string serverPass, string serverRoot)
        {
            client = newClient;
            stream = client.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            serverUsername = serverUser;
            serverPassword = serverPass;
            root = serverRoot;
            currentDirectory = serverRoot + "/";
            userRoot = "/";
        }


        // The handleClient function is the main functino for handling what the client requests.
        // It receives the requests from the client and then takes the correct actions for each request. 
        public void handleClient(object obj)
        {
            writer.WriteLine("220 Service Ready");
            writer.Flush();

            string clientInput;
            
            try
            {
                while(!string.IsNullOrEmpty(clientInput = reader.ReadLine()))
                {
                    //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", clientInput + System.Environment.NewLine);
                    string response = null;
                    string[] clientCommand = clientInput.Split(' ');

                    string command = clientCommand[0].ToUpperInvariant();
                    string arguments = clientCommand.Length > 1 ? clientInput.Substring(clientCommand[0].Length + 1) : null;

                    if (string.IsNullOrWhiteSpace(arguments))
                        arguments = null;
                    if (response == null)
                    {
                        switch(command)
                        {
                            case "STOR":
                                response = store(arguments);
                                break;
                            case "LIST":
                                response = list(arguments);
                                break;
                            case "TYPE":
                                string[] args = arguments.Split(' ');
                                response = transferType(args[0], args.Length > 1 ? args[1] : null);
                                break;
                            case "PORT":
                                response = clientPort(arguments);
                                break;
                            case "PASV":
                                response = passiveConnection();
                                break;
                            case "USER":
                                response = Username(arguments);
                                break;
                            case "PASS":
                                response = Password(arguments);
                                break;
                            case "RETR":
                                response = Retrieve(arguments);
                                break;
                            case "CWD":
                                response = changeDirectory(arguments);
                                break;
                            case "PWD":
                                response = PWD();
                                break;
                            case "QUIT":
                                response = "221 Service closing connection.";
                                break;
                            case "MKD":
                                response = makeDirectory(arguments);
                                break;
                            case "RMD":
                                response = removeDirectory(arguments);
                                break;
                            case "CDUP":
                                response = changeDirectory("..");
                                break;
                            case "RNFR":
                                renaming = arguments;
                                response = "350 File/Directory Renaming pending further action.";
                                break;
                            case "RNTO":
                                response = renameFileORDir(renaming, arguments);
                                break;
                            case "DELE":
                                response = deleteFile(arguments);
                                break;

                        default:
                                response = "502 Command not implemented.";
                                break;
                        }
                    }

                    if (client == null || !client.Connected)
                        break;
                    else
                    {
                        writer.WriteLine(response);
                        writer.Flush();
                        if (response.StartsWith("221"))
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Disconnect();
            }
            Disconnect();
        }

        // Delete file function
        // This function is for when the client requests to delete a file in the server's shared directory
        private string deleteFile(string path)
        {
            try
            {
                if (path.StartsWith(@"\") || path.StartsWith("/"))
                    path = path.Substring(1);

                if (currentDirectory.EndsWith(@"\") || currentDirectory.EndsWith("/"))
                    path = currentDirectory + path;
                else
                    path = currentDirectory + @"\" + path;

                if (File.Exists(path))
                    File.Delete(path);
                else
                    return "550 File not found, delete operation aborted.";

                return "250 Deletion of file was successfull.";
            }
            catch
            {
                hadFailure = true;
            }
            return "550 Delete operation failed.";
        }

        // Rename function
        // This function is for when the client requests to change the name of a file/folder in the shared folder
        private string renameFileORDir(string oldName, string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
                    return "450 Insufficient data, failed to rename file/directory.";

                // build the path for the oldName
                if (oldName.StartsWith(@"\") || oldName.StartsWith("/"))
                    oldName = oldName.Substring(1);

                if (currentDirectory.EndsWith(@"\") || currentDirectory.EndsWith("/"))
                    oldName = currentDirectory + oldName;
                else
                    oldName = currentDirectory + @"\" + oldName;

                // Build the path for the newName
                if (newName.StartsWith(@"\") || newName.StartsWith("/"))
                    newName = newName.Substring(1);

                if (currentDirectory.EndsWith(@"\") || currentDirectory.EndsWith("/"))
                    newName = currentDirectory + newName;
                else
                    newName = currentDirectory + @"\" + newName;

                if (File.Exists(oldName))
                    File.Move(oldName, newName);
                else if (Directory.Exists(oldName))
                    Directory.Move(oldName, newName);
                else
                    return "450 Failed to rename the file or directory, the file or directory does not exist.";

                return "250 File or Directory has been successfully renamed.";
            }
            catch
            {
                hadFailure = true;
            }

            return "450 Rename operation failed.";
        }

        // Remove a directory
        // This function is used to remove a directory that the client requests to delete
        private string removeDirectory(string path)
        {
            try
            {
                if (path.StartsWith(@"\") || path.StartsWith("/"))
                    path = path.Substring(1);

                if (currentDirectory.EndsWith(@"\") || currentDirectory.EndsWith("/"))
                    path = currentDirectory + path;
                else
                    path = currentDirectory + @"\" + path;

                if (Directory.Exists(path))
                    Directory.Delete(path);
                else
                    return "550 Directory not found, command aborted";

                return "250 Directory was successfully deleted.";
            }
            catch
            {
                hadFailure = true;
            }
            return "550 Remove Directory operation failed.";
        }

        // Make the directory of the MKD command
        // This function creates a directory when the client sends the MKD command. 
        private string makeDirectory(string path)
        {
            try
            {
                if (path.StartsWith(@"\") || path.StartsWith("/"))
                    path = path.Substring(1);

                if (currentDirectory.EndsWith(@"\") || currentDirectory.EndsWith("/"))
                    path = currentDirectory + path;
                else
                    path = currentDirectory + @"\" + path;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                else
                    return "550 Directory already exists, process aborted.";

                return "250 Directory created.";
            }
            catch
            {
                hadFailure = true;
            }
            return "550 Create new directory operation failed.";
        }

        // Upload for the store command
        // This function handles the upload requests from the client. 
        private void upload(IAsyncResult result)
        {
            try
            {
                long bytesTransfered = 0;
                if (transferMode == "Active")
                    dataTrasnferClient.EndConnect(result);
                else
                    dataTrasnferClient = passiveListen.EndAcceptTcpClient(result);

                string filepath = (string)result.AsyncState;

                //Setup a data stream to get the data from the client
                using (NetworkStream streamClient = dataTrasnferClient.GetStream())
                {
                    using (FileStream localFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
                    {
                        bytesTransfered = streamTostream(streamClient, localFile);
                    }
                }
                //Close the data connection
                if (dataTrasnferClient != null)
                    dataTrasnferClient.Close();
                dataTrasnferClient = null;
                // Reply that it is done
                writer.WriteLine("226 Close data connection, file transfer completed. {0} bytes received.", bytesTransfered);
                writer.Flush();
            }
            catch
            {
                hadFailure = true;
            }
        }

        // Store command handler
        // This function starts the process of the client uploading a file to the server
        private string store(string path)
        {

            try
            {
                if (path.StartsWith(@"\") || path.StartsWith("/"))
                    path = path.Substring(1);

                if (currentDirectory.EndsWith(@"\") || currentDirectory.EndsWith("/"))
                    path = currentDirectory + path;
                else
                    path = currentDirectory + @"\" + path;
                //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", path + " is the path in store " + System.Environment.NewLine);

                if (path.StartsWith(root))
                {

                    if (transferMode == "Active")
                    {
                        dataTrasnferClient = new TcpClient();
                        dataTrasnferClient.BeginConnect(dataEndpoint.Address, dataEndpoint.Port, upload, path);
                    }
                    else
                        passiveListen.BeginAcceptTcpClient(upload, path);
                }
                else
                    return "426 Connection closed, file trasnfer aborted. Destination path does not exist.";

                return string.Format("150 Opening {0} mode for data trasnfer for command STOR", transferMode);
            }
            catch
            {
                hadFailure = true;
            }
            return "426 Upload operation failed.";
        }

        // Return the current working directory
        // This function returns the currnet working directory to the client
        private string PWD()
        {
            try
            {
                string tempCWD = "";

                currentDirectory = currentDirectory.Replace("/", @"\");
                // This if statement should be entered.
                if (currentDirectory.StartsWith(root))
                {
                    tempCWD = currentDirectory.Remove(0, root.Length);
                    tempCWD = tempCWD.Replace(@"\", "/");
                }
                else // This shouldn't be reached
                    tempCWD = "/";

                if (tempCWD == "")
                    tempCWD = "/";

                //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", tempCWD + " is the cwd in the pwd command handler "  + System.Environment.NewLine);
                return string.Format("257 {0} is the current directory.", tempCWD);
            }
            catch
            {
                hadFailure = true;
            }
            return "Print working directory operation failed.";
        }

        // Method for chaning the current directory
        // This function changes the current working directory of the server.
        private string changeDirectory(string path)
        {
            try
            {
                if (path == "/")
                    currentDirectory = root + "/";
                else
                {
                    string newCWD;

                    /*
                    if(path.StartsWith("/"))
                    {
                        path = path.Substring(1).Replace('/', '\\');
                        newCWD = Path.Combine(root, path);
                    }
                    else
                    {
                        path = path.Replace('/', '\\');
                        newCWD = Path.Combine(root, path);
                    }
                    */

                    //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", root + " Name of ROOT before building " + System.Environment.NewLine);
                    if (path.StartsWith("/") || path.StartsWith(@"\"))
                        newCWD = root + path + @"\";
                    else
                        newCWD = currentDirectory + @"\" + path;


                    //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", newCWD + " Name of newCWD after building " + System.Environment.NewLine);
                    //string temp = Path.Combine(root, newCWD);
                    //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog.txt", temp + " Name of temp" + System.Environment.NewLine);
                    if (Directory.Exists(newCWD))
                    {
                        currentDirectory = new DirectoryInfo(newCWD).FullName;

                        if (!currentDirectory.StartsWith(root))
                        {
                            currentDirectory = root + "/";
                        }
                    }
                    else
                        currentDirectory = root + "/";
                    //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", currentDirectory + " cwd before getting rid of root" + System.Environment.NewLine);

                }
                return "250 Changed to the new directory";
            }
            catch
            {
                hadFailure = true;
            }
            return "504 Change directory operation failed.";
        }

        // Used for transfering data from the file stream to the datastream or vice-versa
        private long streamTostream(Stream inputStream, Stream outputStream)
        {
            try
            {
                // Will keep track of bytes transfered.
                long totalBytes = 0;
                int countBytes = 0;
                // Used for Image transfer
                byte[] bufferImage = new byte[4096];
                // Used for ASCII transfer
                char[] bufferASCII = new char[4096];

                // Check transfer type and send accordinly
                if (transferTypeCode == "A")  // ASCII Transfer
                {
                    // Setup the reader and writer between the two streams
                    using (StreamReader read = new StreamReader(inputStream))
                    {
                        // Set the writer to ASCII encoding because of the transfertypecode
                        using (StreamWriter write = new StreamWriter(outputStream, Encoding.ASCII))
                        {
                            while ((countBytes = read.Read(bufferASCII, 0, bufferASCII.Length)) > 0)
                            {
                                write.Write(bufferASCII, 0, countBytes);
                                totalBytes = totalBytes + countBytes;
                            }
                        }
                    }
                }
                else  //Image Transfer
                {
                    while ((countBytes = inputStream.Read(bufferImage, 0, bufferImage.Length)) > 0)
                    {
                        outputStream.Write(bufferImage, 0, countBytes);
                        totalBytes = totalBytes + countBytes;
                    }
                }

                // Return the totalBytes transfered.
                return totalBytes;
            }
            catch
            {
                hadFailure = true;
            }
            return 0;
        }

        // Perform the client's download
        // This function handles the download request from the client and delivers the file to the client
        private void download(IAsyncResult result)
        {
            try
            {
                long bytesTransfered = 0;
                if (transferMode == "Active")
                    dataTrasnferClient.EndConnect(result);
                else
                    dataTrasnferClient = passiveListen.EndAcceptTcpClient(result);

                string path = (string)result.AsyncState;

                // Setup a data stream to send the data to the client
                using (NetworkStream streamClient = dataTrasnferClient.GetStream())
                {
                    using (FileStream streamFile = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        // Call streamTostream to handle the transfer
                        bytesTransfered = streamTostream(streamFile, streamClient);
                    }
                }

                // Close the client connection and null it
                dataTrasnferClient.Close();
                dataTrasnferClient = null;

                // Reply on the command connection
                writer.WriteLine("226 Close data connection, file transfer completed. {0} Bytes trasnfered.", bytesTransfered);
                writer.Flush();
            }
            catch
            {
                hadFailure = true;
            }
        }

        // Client download handler
        // This function starts the process of the delivering the requested file from the server to the client
        private string Retrieve(string path)
        {
            try
            {
                if (path == null)
                    path = currentDirectory;
                //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", path + " Name of command reaching retrieve function"  + System.Environment.NewLine);
                /*
                 // Server root needs to be listed.
                 if (path == "/")
                 {
                     path = root;
                 }
                 else if (path.StartsWith("/"))
                 {
                     path = new FileInfo(Path.Combine(root, path.Substring(1))).FullName;
                 }
                 else if (currentDirectory == "/")
                 {
                     path = new FileInfo(Path.Combine(root, path)).FullName;
                 }
                 else
                     //path = new FileInfo(Path.Combine(root, currentDirectory, path)).FullName;
                     path = new FileInfo(root + currentDirectory +  @"\" + path).FullName;
                     */

                path = currentDirectory + "/" + path;

                //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", path + " Name of path before filepath check" + System.Environment.NewLine);

                if (path.StartsWith(root))
                {
                    if (File.Exists(path))
                    {
                        if (transferMode == "Active")
                        {
                            dataTrasnferClient = new TcpClient();
                            dataTrasnferClient.BeginConnect(dataEndpoint.Address, dataEndpoint.Port, download, path);
                        }
                        else
                            passiveListen.BeginAcceptTcpClient(download, path);

                        return string.Format("150 Opening {0} mode for transfer for command RETR", transferMode);
                    }
                }
                return "550 File not found in directory";
            }
            catch
            {
                hadFailure = true;
            }
            return "550 Download operation failed.";
        }

        // Build the list for the list command handler
        // This funciton lists the items in the directory on the server and delivers then to the client
        private void buildList(IAsyncResult result)
        {
            try
            {
                if (transferMode == "Active")
                    dataTrasnferClient.EndConnect(result);
                else
                    dataTrasnferClient = passiveListen.EndAcceptTcpClient(result);

                string dirpath = (string)result.AsyncState;

                // Setup a stream to be used between the client and server
                using (NetworkStream stream = dataTrasnferClient.GetStream())
                {
                    readData = new StreamReader(stream, Encoding.ASCII);
                    writeData = new StreamWriter(stream, Encoding.ASCII);

                    // Get all the directories in this path
                    IEnumerable<string> directory = Directory.EnumerateDirectories(dirpath);
                    foreach (string dir in directory)
                    {
                        DirectoryInfo direct = new DirectoryInfo(dir);

                        string date = direct.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ? direct.LastWriteTime.ToString("MMM dd yyyy") : direct.LastWriteTime.ToString("MMM dd HH:mm");
                        string entry = string.Format("drw-rw-rw   2 ftp      ftp    {0,8} {1} {2}", "4096", date, direct.Name);
                        //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", entry + " string of dir in list" + System.Environment.NewLine);
                        writeData.WriteLine(entry);
                        writeData.Flush();
                    }

                    // Get all the files in the directories in this path
                    IEnumerable<string> dirFiles = Directory.EnumerateFiles(dirpath);
                    foreach (string file in dirFiles)
                    {
                        FileInfo files = new FileInfo(file);

                        string date = files.LastWriteTime > DateTime.Now - TimeSpan.FromDays(180) ? files.LastWriteTime.ToString("MMM dd yyyy") : files.LastWriteTime.ToString("MMM dd HH:mm");
                        string entry = string.Format("-rw-rw-rw-        2 ftp      ftp    {0,8} {1} {2}", files.Length, date, files.Name);
                        writeData.WriteLine(entry);
                        writeData.Flush();
                    }
                }

                dataTrasnferClient.Close();
                dataTrasnferClient = null;

                writer.WriteLine("226 Transfer completed successfully");
                writer.Flush();
            }
            catch
            {
                hadFailure = true;
            }
        }

        // Handle the List command
        // This funciton begins the process for delivering the list of the directory to the client
        private string list(string path)
        {
            try
            {
                /*// Server root needs to be listed.
                    if (path == "/")
                    {
                        path = root;
                    }
                    else if (path.StartsWith("/") || path.StartsWith("\\"))
                    {
                        path = new FileInfo(Path.Combine(root, path.Substring(1))).FullName;
                    }
                    else
                        path = new FileInfo(Path.Combine(root, currentDirectory, path)).FullName;
                        */

                // Build the path for listing

                if (path == null)
                {
                    // Do nothing
                }
                else if (path == "/")
                    currentDirectory = root + "/";
                else if (path.StartsWith("/") || path.StartsWith(@"\"))
                    currentDirectory = currentDirectory + path;
                else
                    currentDirectory = currentDirectory + "/" + path;

                //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", path + " Name of argument  path in LIST" + System.Environment.NewLine);

                //System.IO.File.AppendAllText(@"C:\Users\USER\Desktop\serverlog\serverlog.txt", currentDirectory + " Name of path in LIST" + System.Environment.NewLine);

                if (currentDirectory.StartsWith(root))
                {
                    if (transferMode == "Active")
                    {
                        dataTrasnferClient = new TcpClient();
                        dataTrasnferClient.BeginConnect(dataEndpoint.Address, dataEndpoint.Port, buildList, currentDirectory);
                    }
                    else
                        passiveListen.BeginAcceptTcpClient(buildList, currentDirectory);
                }

                return string.Format("150 Opening {0} mode for data trasnfer for command LIST", transferMode);
            }
            catch
            {
                hadFailure = true;
            }

            return "504 List operation failed.";

        }

        // Passive command handler
        // 
        private string passiveConnection()
        {
            try
            {
                transferMode = "Passive";

                IPAddress IP = ((IPEndPoint)client.Client.LocalEndPoint).Address;

                passiveListen = new TcpListener(IP, 0);
                passiveListen.Start();

                IPEndPoint passiveEndpoint = (IPEndPoint)passiveListen.LocalEndpoint;

                byte[] addr = passiveEndpoint.Address.GetAddressBytes();
                short port = (short)passiveEndpoint.Port;

                byte[] arrayPort = BitConverter.GetBytes(port);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(arrayPort);

                return string.Format("227 Enter Passive mode ({0},{1},{2},{3},{4},{5})", addr[0], addr[1], addr[2], addr[3], arrayPort[0], arrayPort[1]);
            }
            catch
            {
                hadFailure = true;
            }
            return "504 Passive operation failed.";
        }

        // Port command handler
        // Setting up the connection to the client using the provided information.
        private string clientPort(string info)
        {
            try
            {
                transferMode = "Active";

                string[] splitInfo = info.Split(',');

                byte[] IP = new byte[4];
                byte[] portNum = new byte[2];

                // Get IP adress from input
                for (int i = 0; i < 4; i++)
                {
                    IP[i] = Convert.ToByte(splitInfo[i]);
                }
                // Get port from input
                for (int i = 4; i < 6; i++)
                {
                    portNum[i - 4] = Convert.ToByte(splitInfo[i]);
                }

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(portNum);
                dataEndpoint = new IPEndPoint(new IPAddress(IP), BitConverter.ToInt16(portNum, 0));

                return "200 Data connection established";
            }
            catch (Exception)
            {
                hadFailure = true;
            }
            return "504 Port operation failed.";
        }

        // Adjust the type for transfer modes
        // Different modes are needed for different files, for example .exe should be transfered as Image.
        private string transferType(string type, string format)
        {
            try
            {
                string response = "";

                switch (type)
                {
                    case "A":
                        transferTypeCode = type;
                        response = "200 Transfer type set to ASCII";
                        break;
                    case "I":
                        transferTypeCode = type;
                        response = "200 Trasnfer type set to Image";
                        break;
                    default:
                        response = "504 Type not supported. This server only supports ASCII and Image.";
                        break;
                }

                if (format != null)
                {
                    switch (format)
                    {
                        case "N":
                            response = "200 Format set to NonPrint";
                            break;
                        default:
                            response = " 504 Format not supported. This server only supports NonPrint";
                            break;
                    }
                }

                return response;
            }
            catch
            {
                hadFailure = true;
            }
            return "504 Type operation failed.";
        }

        // Accept and set the username for the client.
        // Test the username provided by the client against the username that the server is expecting
        private string Username(string user)
        {
            try
            {
                username = user;
                return "331 Username accepted, send password.";
            }
            catch (Exception)
            {
                hadFailure = true;
            }
            return "530 Username function failed.";
        }

        // Accept and set the password for the client
        // Test the password provided by the client against the password that the server is expecting
        private string Password(string pass)
        {
            try
            {
                password = pass;
                if (username == serverUsername && password == serverPassword)
                    return "230 User logged in";
                else
                    return "530 User not logged in, incorrect username/password.";
            }
            catch
            {
                hadFailure = true;
            }
            return "530 User not logged in, Password function failed.";
        }

        // Change the directory to what the client specifies.
        // Function changes the current working directory to what the client specifies
        private string changeCWD(string path)
        {
            return "250 Change to new directory.";
        }

        // Disconnect from client
        // Function disconnects the client and the server. Shutting down the data streams
        public void Disconnect()
        {
            try
            {
                if (writer != null)
                    writer.Close();
                if (stream != null)
                    stream.Close();
                if (reader != null)
                    reader.Close();
                if (client != null)
                    client.Close();
                if (dataTrasnferClient != null)
                    dataTrasnferClient.Close();
            }
            catch
            {
                hadFailure = true;
            }
        }
    }
}
