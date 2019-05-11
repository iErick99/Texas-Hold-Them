using System;
using System.Net;

namespace Server
{
    class Program {
        static void Main(string[] args) {
            // Aca puse la dir. IP loopback por mientras para que no tengan que estarla cambiando al correrlo
            Server server = new Server(IPAddress.Loopback.ToString(), 100);

            server.Start();
            server.Run();
        }
    }
}
