<<<<<<< HEAD
﻿using System;
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
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();
            controller.inicio();
        }
    }
}
>>>>>>> Juego_server
