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
                    if (controller.JugadoresEnLinea() != 4)
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
            Jugador jugador = new Jugador();
            jugador.Client = client;

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
                                    String user = (string)deserializedRequest.user;
                                    AD.authentication(user, (string)deserializedRequest.password);
                                    response = "{\"success\":true}";

                                    if (controller.Jugadores.Count == 4)
                                    {
                                        foreach (Jugador jugador2 in controller.Jugadores)
                                        {
                                            if (user == jugador2.Nombre)
                                            {
                                                jugador2.EnLinea = true;
                                                jugador2.Client = client;
                                                response = "{\"success\":true}";
                                                break;
                                            }
                                            else
                                            {
                                                response = "{\"success\":false}";
                                            }
                                            
                                        }
                                    }
                                    else
                                    {
                                        jugador.Nombre = (string)deserializedRequest.user;
                                        jugador.EnLinea = true;
                                        controller.Jugadores.Add(jugador);
                                    }

                                    if (controller.Jugadores.Count == 4 && controller.PartidaIniciada != true)
                                    {
                                        controller.inicio();
                                    }
                                }
                                catch (Exception e)
                                {
                                    response = "{\"success\":false}";
                                    Console.WriteLine(e.Message.ToString());
                                }

                                responseBuffer = Encoding.ASCII.GetBytes(response);
                                dataStream.Write(responseBuffer, 0, responseBuffer.Length);
                                Console.WriteLine(response);
                                dataStream.Flush();
                                SendGameInformation();
                                break;
                            }

                        case "raise":
                            {
                                int apuesta = deserializedRequest.quantity;
                                controller.apostar(jugador.Nombre, "Apostar", apuesta);
                                SendGameInformation();
                            }
                            break;

                        case "create":
                            {
                                try
                                {
                                    String user = (string)deserializedRequest.user;
                                    String password = (string)deserializedRequest.password;

                                    foreach(Jugador jugadorCrear in controller.Jugadores)
                                    {
                                        if(jugadorCrear.Nombre == user)
                                        {
                                            throw new Exception("Este usuario ya está logueado.");
                                        }
                                    }

                                    AD.createUser(user, user, password);
                                    response = "{\"success\":true}";
                                }
                                catch (Exception e)
                                {
                                    response = "{\"success\":false}";
                                    Console.WriteLine(e.Message.ToString());
                                }

                                responseBuffer = Encoding.ASCII.GetBytes(response);
                                dataStream.Write(responseBuffer, 0, responseBuffer.Length);
                                Console.WriteLine(response);
                                dataStream.Flush();

                                break;
                            }

                        case "pass":
                            {
                                controller.apostar(jugador.Nombre, "Pasar", 0);
                                SendGameInformation();
                            }
                            break;

                        case "fold":
                            {
                                controller.apostar(jugador.Nombre, "Botar", 0);
                                SendGameInformation();
                            }
                            break;

                        case "call":
                            {
                                controller.apostar(jugador.Nombre, "Igualar", 0);
                                SendGameInformation();
                            }
                            break;
                        case "disconnect":
                            {
                                while (true)
                                {
                                    if (controller.Turno == jugador.Nombre)
                                    {
                                        jugador.EnLinea = false;
                                        controller.apostar(jugador.Nombre, "Botar", 0);
                                        SendGameInformation();
                                        break;
                                    }
                                }
                            }
                            break;

                        default: response = "{\"success\":false}"; break;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("{0} has disconnected", jugador.Nombre));
                Console.WriteLine(String.Format("{0} has disconnected", clientAddress));
            }
        }

        public void SendGameInformation()
        {
            String informacion;

            if (controller.Jugadores.Count != 4)
            {
                informacion = "{ 'dealer': '" + controller.dealer + "', 'players': [ ";
            }
            else
            {
                informacion = "{ 'dealer': '" + controller.dealer + "', 'turn': '" + controller.Turno + "', 'players': [ ";
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

            informacion += "], 'table': { 'pot': " + controller.Pozo  + " , 'cards': [";
            for (int i = 0; i < controller.Cartas.Mesa.Count; i++)
            {
                informacion += "{ 'number': " + controller.Cartas.Mesa[i].getNumero() + ", 'symbol': '" + controller.Cartas.Mesa[i].getSimbolo() + "'}";
                if (i != controller.Cartas.Mesa.Count - 1)
                    informacion += ",";
            }

            informacion += "] } }";

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