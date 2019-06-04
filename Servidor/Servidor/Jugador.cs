using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Servidor 
{
    class Jugador
    {
        private string nombre;
        private string clave;
        private int numeroJugador;

        private int apostado;
        private int monto;

        private Carta carta1;
        private Carta carta2;

        private bool jugando;

        private TcpClient client;

        public Jugador()
        {
            nombre = "";
            clave = "";
            monto = 1000;
            apostado = 0;
            carta1 = null;
            carta2 = null;
            jugando = true;
        }
        public TcpClient Client
        {
            get { return client; }
            set { client = value; }
        }
        public int NumeroJugador
        {
            get { return numeroJugador; }
            set { numeroJugador = value; }
        }
        public string getNombre() { return nombre; }
        public string getClave() { return clave; }
        public int getMonto() { return monto; }
        public int getApostado() { return apostado; }
        public Carta getCarta1() { return carta1; }
        public Carta getCarta2() { return carta2; }
        public bool getJugando() { return jugando; }
        public void setMonto(int mon) { monto = mon; }
        public void setApostado(int apos) { apostado = apos; }
        public void setCarta1(Carta c) { carta1 = c; }
        public void setCarta2(Carta c) { carta2 = c; }
        public void setJugando(bool j) { jugando = j; }
    }
}
