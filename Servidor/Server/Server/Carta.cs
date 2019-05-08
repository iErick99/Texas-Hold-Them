using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Carta
    {
        private string numero;
        private string simbolo;

        public Carta()
        {
            numero = "";
            simbolo = "";
        }

        public Carta(string num, string sim)
        {
            numero = num;
            simbolo = sim;
        }
        public string getNumero() { return numero; }
        public string getSimbolo() { return simbolo; }
        public void setNumero(string num) { numero = num; }
        public void setSimbolo(string sim) { simbolo = sim; }
    }
}
