using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            client.Connect("172.17.30.145", 100);
            Console.WriteLine(client.SendRequest(Console.ReadLine()));
            Console.WriteLine(client.SendRequest("{\"method\": \"login\"}"));
            Console.ReadLine();
        }
    }
}
