using System;
using System.Net.Sockets;
using System.Text;

namespace Poker
{
    class Client
    {
        private TcpClient socket = new TcpClient();

        public void Connect(string address, int port)
        {
            int attempts = 0;

            while (!socket.Connected)
            {
                try
                {
                    attempts++;
                    socket.Connect(address, port);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine(String.Format("Connection attempts: {0}" , attempts.ToString()));
                }
            }

            Console.WriteLine("Connection established!");
        }

        // Client request sender method. It returns the server's response as a string (JSON format)
        public string SendRequest(string request)
        {
            NetworkStream dataStream;
            int responseSize;
            string response = String.Empty;
            byte[] requestBuffer;
            byte[] responseBuffer;

            try
            {
                // Encode and send request to server
                dataStream = socket.GetStream();
                requestBuffer = Encoding.ASCII.GetBytes(request);
                dataStream.Write(requestBuffer, 0, requestBuffer.Length);
                dataStream.Flush();

                // Parse and print server's response
                responseBuffer = new byte[2048];
                responseSize = dataStream.Read(responseBuffer, 0, responseBuffer.Length);

                response = Encoding.ASCII.GetString(responseBuffer, 0, responseSize);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            return response;
        }
    }
}
