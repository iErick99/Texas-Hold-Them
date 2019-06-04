using System;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            client.Connect("13.90.205.129", 100);

            while (true)
            {
                Thread.Sleep(5000);
                client.SendData(String.Format("{{\"method\": \"raise\", \"cantidad\": {0}}}", 5));
                Console.WriteLine(client.GetData());
            }
        }
    }
}
