using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Carta
    {
        private int numero;
        private string simbolo;

        public Carta()
        {
            numero = 0;
            simbolo = "";
        }

        public Carta(int num, string sim)
        {
            numero = num;
            simbolo = sim;
        }
        public int getNumero() { return numero; }
        public string getSimbolo() { return simbolo; }
        public void setNumero(int num) { numero = num; }
        public void setSimbolo(string sim) { simbolo = sim; }
    }
}
