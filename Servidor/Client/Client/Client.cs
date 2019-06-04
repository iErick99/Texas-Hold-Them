using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        private TcpClient socket = new TcpClient();

        // Client server connection method
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

        // Client data getter method
        public string GetData()
        {
            NetworkStream dataStream;
            int responseSize;
            string response = String.Empty;
            byte[] responseBuffer;

            try
            {
                dataStream = socket.GetStream();

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

        // Client data sender method
        public void SendData(string request)
        {
            NetworkStream dataStream;
            byte[] requestBuffer;

            try
            {
                // Encode and send request to server
                dataStream = socket.GetStream();

                requestBuffer = Encoding.ASCII.GetBytes(request);
                dataStream.Write(requestBuffer, 0, requestBuffer.Length);
                dataStream.Flush();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
