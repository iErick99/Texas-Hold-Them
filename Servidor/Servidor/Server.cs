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
        public Controller controller;
        public Thread hController;
        public Server(string address, int port)
        {
            // Initialize server's socket
            socket = new TcpListener(IPAddress.Any, port);
        }

        // Server starting method
        public void Start()
        {
            // Listen connections
            hController = new Thread(new ThreadStart(controller.inicio));
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
            controller.Jugadores = jugadores;
            if (jugadores.Count == 4)
            {
                hController.Start();
            }
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
            string nombreJugador = "";

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
                                    nombreJugador = (string)deserializedRequest.usuario;

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
                                controller.apostar(nombreJugador, "apostar", apuesta);
                                Console.WriteLine(apuesta);
                                SendGameInformation();
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

        public void SendGameInformation()
        {
            /*String informacion = "{ jugadores: [";
            foreach (Jugador jugador in jugadores)
            {
                informacion += "{ nombre:" + jugador.getNombre();
                informacion += ", apuesta:" + jugador.getApostado();
                informacion += ", monto:" + jugador.getMonto();
                informacion += ", carta_1:" + jugador.getCarta1();
                informacion += ", carta_2:" + jugador.getCarta2() + "}";
            }

            //informacion = "mesaCartas: " + controller.mo  + "}";

            informacion += "]}";*/

            String informacion = "{ 'jugadores': [ { 'nombre': 'Faziop', 'carta1': { 'numero': 2, 'simbolo': 'Corazones' }, 'carta2': { 'numero': 3, 'simbolo': 'Corazones' }, 'saldo': 1000, 'apuesta': 0 }, { 'nombre': 'GonzaCRC', 'carta1': { 'numero': 3, 'simbolo': 'Corazones' }, 'carta2': { 'numero': 4, 'simbolo': 'Corazones' }, 'saldo': 1000, 'apuesta': 0 }, { 'nombre': 'Bleysh', 'carta1': { 'numero': 5, 'simbolo': 'Corazones' }, 'carta2': { 'numero': 6, 'simbolo': 'Corazones' }, 'saldo': 1000, 'apuesta': 0 }, { 'nombre': 'iErick99', 'carta1': { 'numero': 7, 'simbolo': 'Corazones' }, 'carta2': { 'numero': 8, 'simbolo': 'Corazones' }, 'saldo': 800, 'apuesta': 200 } ], 'mesa': { 'carta1': { 'numero': 5, 'simbolo': 'Treboles' }, 'carta2': { 'numero': 6, 'simbolo': 'Treboles' }, 'carta3': { 'numero': 7, 'simbolo': 'Treboles' }, 'carta4': { 'numero': 8, 'simbolo': 'Treboles' }, 'carta5': { 'numero': 9, 'simbolo': 'Treboles' } } }";

            BroadCast(informacion);
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