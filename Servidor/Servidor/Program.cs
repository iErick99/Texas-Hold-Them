using System;
using System.Net;

namespace Servidor 
{
    class Program {
        static void Main(string[] args) {
            // Aca puse la dir. IP loopback por mientras para que no tengan que estarla cambiando al correrlo
            Server server = new Server(IPAddress.Any.ToString(), 100);

            //server.Start();
            //server.Run();


            Controller controller = new Controller();
            controller.inicio();
        }
    }
}
