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
                Console.WriteLine(client.AcceptBroadcast());
            }
        }
    }
}
