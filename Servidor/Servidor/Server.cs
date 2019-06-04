using System;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Connection;
using System.Collections.Generic;

namespace Servidor
{
    class Server
    {
        private TcpListener socket;
        private ActiveDirectory AD = new ActiveDirectory();
        List<Jugador> jugadores = new List<Jugador>();
        int numeroJugador = 1;

        public Server(string address, int port)
        {
            // Initialize server's socket
            socket = new TcpListener(IPAddress.Any, port);
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
                    if (jugadores.Count != 4)
                    {
                        TcpClient client = socket.AcceptTcpClient();
                        this.CreateClientThread(client);
                        Console.WriteLine(String.Format("{0} has connected! Waiting for request...", (client.Client.RemoteEndPoint).ToString()));
                    }
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
            Jugador jugador = new Jugador();
            jugador.Client = client;
            jugador.NumeroJugador = numeroJugador;
            jugadores.Add(jugador);
            Thread clientThread = new Thread(() => ReceiveRequests(jugador.Client));
            numeroJugador += 1;
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

                    // TODO: Definir mas metodos del servidor
                    switch ((string)deserializedRequest.method)
                    {
                        case "login":
                            {
                                try
                                {
                                    AD.authentication((string)deserializedRequest.usuario, (string)deserializedRequest.password);
                                    response = "{\"success\":true}";

                                }
                                catch (Exception e)
                                {
                                    response = "{\"success\":false}";
                                    Console.WriteLine(e.Message.ToString());
                                }

                                break;
                            }

                        case "raise":
                            {
                                int apuesta = deserializedRequest.raise;
                                Console.WriteLine(apuesta);
                                BroadCast("{dinero: " + apuesta + "}");

                            }
                            break;

                        case "create":
                            {
                                try
                                {
                                    AD.createUser((string)deserializedRequest.usuario, (string)deserializedRequest.password);
                                    response = "{\"success\":true}";
                                }
                                catch (Exception e)
                                {
                                    response = "{\"success\":false}";
                                    Console.WriteLine(e.Message.ToString());
                                }
                                break;
                            }

                        case "pass":
                            {
                            }
                            break;

                        case "fold":
                            {
                            }
                            break;

                        case "call":
                            {
                            }
                            break;

                        default: response = "{\"success\":false}"; break;
                    }

                    responseBuffer = Encoding.ASCII.GetBytes(response);

                    dataStream.Write(responseBuffer, 0, responseBuffer.Length);
                    Console.WriteLine(response);
                    dataStream.Flush();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("{0} has disconnected", clientAddress));
            }
        }

        public void BroadCast(String response)
        {
            for (int i = 0; i < jugadores.Count; i++)
            {

                NetworkStream dataStream;
                byte[] responseBuffer;
                dataStream = jugadores[i].Client.GetStream();

                responseBuffer = Encoding.ASCII.GetBytes(response);

                dataStream.Write(responseBuffer, 0, responseBuffer.Length);
                dataStream.Flush();
            }
        }

    }
}