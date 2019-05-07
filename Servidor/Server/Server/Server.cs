using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Server
{
    class Server
    {
        private TcpListener socket;

        public Server(string address, int port)
        {
            // Initialize server's socket
            socket = new TcpListener(IPAddress.Parse(address), port);
        }

        // Server starting method
        public void Start()
        {
            // Listen connections
            socket.Start();

            Console.WriteLine(String.Format("Server started on {0}...", socket.LocalEndpoint));
        }

        // Client connections receiver method
        public void Run()
        {
            try
            {
                while (true)
                {
                    TcpClient client = socket.AcceptTcpClient();
                    this.CreateClientThread(client);

                    Console.WriteLine(String.Format("{0} has connected! Waiting for request...", (client.Client.RemoteEndPoint).ToString()));
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                socket.Stop();
            }            
        }

        public void CreateClientThread(TcpClient client)
        {
            Thread clientThread = new Thread(() => ReceiveRequests(client));

            clientThread.Start();
        }

        // Clients' request parsing method
        public void ReceiveRequests(TcpClient client)
        {
            NetworkStream dataStream;
            int requestSize;
            string clientAddress = (client.Client.RemoteEndPoint).ToString();
            string request;
            string response;
            byte[] requestBuffer;
            byte[] responseBuffer;

            try
            {
                while (true)
                {
                    // Parse client's received data
                    dataStream = client.GetStream();

                    requestBuffer = new byte[2048];
                    requestSize = dataStream.Read(requestBuffer, 0, requestBuffer.Length);

                    request = Encoding.ASCII.GetString(requestBuffer, 0, requestSize);

                    Console.WriteLine("Receiving client's data...");
                    Console.WriteLine(String.Format("Received data: {0}", request));

                    // Encode and send server's response
                    response = string.Empty;

                    var deserializedRequest = JsonConvert.DeserializeObject<dynamic>(request);

                    switch ((string)deserializedRequest.method)
                    {
                        case "login":
                            response =
                                "{" +
                                "\"success\": true" +
                                "}";
                            break;

                        default:
                            response =
                                "{" +
                                "\"success\": false" +
                                "}";
                            break;
                    }

                    responseBuffer = Encoding.ASCII.GetBytes(response);

                    dataStream.Write(responseBuffer, 0, responseBuffer.Length);
                    dataStream.Flush();
                }
            }
            catch (Exception)
            {
                Console.WriteLine(String.Format("{0} has disconnected", clientAddress));
            }
        }
    }
}