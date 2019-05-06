using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Poker {
    class Cliente {
        private string ip;
        private int puerto;
        private TcpClient cliente;

        public Cliente(string ip, int puerto) {
            this.ip = ip;
            this.puerto = puerto;
            this.cliente = new TcpClient();

            //Console.Write("Enter the string to be transmitted : ");

            //String str = Console.ReadLine();
            //Stream stm = cliente.GetStream();

            //ASCIIEncoding asen = new ASCIIEncoding();
            //byte[] ba = asen.GetBytes(str);
            //Console.WriteLine("Transmitting...");

            //stm.Write(ba, 0, ba.Length);

            //byte[] bb = new byte[100];
            //int k = stm.Read(bb, 0, 100);

            //for (int i = 0; i < k; i++) {
            //    Console.Write(Convert.ToChar(bb[i]));
            //}

            //Console.Read();

        }

        public void conectar() {
            cliente.Connect(ip, puerto);
        }

        public void desconectar() {
            cliente.Close();
        }
    }
}
