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
        public Controller controller;
        public int contadorTurno = 0;
       // public Thread hController;
        public Server(string address, int port)
        {
            controller = new Controller();
            // Initialize server's socket
            socket = new TcpListener(IPAddress.Any, port);
        }

        // Server starting method
        public void Start()
        {
            // Listen connections
            //hController = new Thread(new ThreadStart(controller.inicio));
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
                    if (controller.Jugadores.Count != 4)
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
            controller.Jugadores.Add(jugador);
            if (controller.Jugadores.Count == 4)
            {
                controller.repartirCartas();
            }
            Thread clientThread = new Thread(() => ReceiveRequests(jugador));
            clientThread.Start();
        }

        // Clients' request parsing method
        public void ReceiveRequests(Jugador jugador)
        {
            NetworkStream dataStream;
            int requestSize;
            string clientAddress = (jugador.Client.Client.RemoteEndPoint).ToString();
            string request;
            string response;
            byte[] requestBuffer;
            byte[] responseBuffer;

            try
            {
                while (true)
                {
                    // Parse client's received data
                    dataStream = jugador.Client.GetStream();

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
                                    //AD.authentication((string)deserializedRequest.user, (string)deserializedRequest.password);
                                    response = "{\"success\":true}";
                                    responseBuffer = Encoding.ASCII.GetBytes(response);
                                    dataStream.Write(responseBuffer, 0, responseBuffer.Length);
                                    Console.WriteLine(response);
                                    dataStream.Flush();
                                    jugador.Nombre = (string)deserializedRequest.user;
                                    SendGameInformation();
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
                                //controller.apostar(nombreJugador, "apostar", apuesta);
                                Console.WriteLine(apuesta);
                            }
                            break;

                        case "create":
                            {
                                try
                                {
                                    AD.createUser((string)deserializedRequest.name, (string)deserializedRequest.usuario, (string)deserializedRequest.password);
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

                        case "prueba":
                            {
                                contadorTurno += 1;
                                SendGameInformation();
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

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("{0} has disconnected", clientAddress));
            }
        }

        public void SendGameInformation()
        {
            //String informacion = "{ 'dealer': 'salu4', 'turn': 'iErick99', 'players': [ ";

            //Borrar esta basura
            String informacion;

            if (contadorTurno == 3)
                contadorTurno = 0;

            if (controller.Jugadores.Count != 4)
            {
                informacion = "{ 'dealer': 'salu4', 'players': [ ";
            }
            else
            {
                informacion = "{ 'dealer': 'salu4', 'turn': '" + controller.Jugadores[contadorTurno].Nombre + "', 'players': [ ";
            }

            //
            for (int i = 0; i < controller.Jugadores.Count; i++)
            {
                informacion += "{ 'name': '" + controller.Jugadores[i].Nombre + "'";
                informacion += ", 'cards': [";
                if (controller.Jugadores[i].getCarta1() != null && controller.Jugadores[i].getCarta2() != null)
                {
                    informacion += " { 'number': " + controller.Jugadores[i].getCarta1().getNumero() + ", 'symbol': '" + controller.Jugadores[i].getCarta1().getSimbolo() + "'}";
                    informacion += ", { 'number': " + controller.Jugadores[i].getCarta2().getNumero() + ", 'symbol': '" + controller.Jugadores[i].getCarta2().getSimbolo() + "'}";
                }
                informacion += "]";
                informacion += ", 'balance': " + controller.Jugadores[i].getMonto();
                informacion += ", 'bet': " + controller.Jugadores[i].getApostado() + "}";
                if (i != controller.Jugadores.Count - 1)
                    informacion += ",";
            }

            //informacion = "mesaCartas: " + controller.mo  + "}";

            informacion += "], 'table': { 'pot': 100, 'cards': [ { 'number': 5, 'symbol': 'Treboles' }, { 'number': 6, 'symbol': 'Treboles' }, { 'number': 7, 'symbol': 'Treboles' }, { 'number': 8, 'symbol': 'Treboles' }, { 'number': 9, 'symbol': 'Treboles' } ] } }";

            Console.WriteLine(String.Format("{0}", informacion));

            BroadCast(informacion);
        }

        public void BroadCast(String response)
        {
            for (int i = 0; i < controller.Jugadores.Count; i++)
            {

                NetworkStream dataStream;
                byte[] responseBuffer;
                dataStream = controller.Jugadores[i].Client.GetStream();

                responseBuffer = Encoding.ASCII.GetBytes(response);

                dataStream.Write(responseBuffer, 0, responseBuffer.Length);
                dataStream.Flush();
            }
        }
    }
}