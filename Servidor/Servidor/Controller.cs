using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace Servidor
{
    class Controller
    {
        public int pozo = 0;
        public int apuesta = 0;

        public int apuestaMinima = 0;

        public bool hilo1;
        public bool hilo2;
        public bool hilo3;
        public bool hilo4;

        public string instruccion = "";

        public ModelCartas cartas;

        private List<Jugador> jugadores = new List<Jugador>();
        Server server = new Server(IPAddress.Any.ToString(), 100);

        public List<Jugador> Jugadores
        {
            get { return jugadores; }
            set { jugadores = value; }
        }

        public Controller()
        {
            server.Start();
            server.Run();
            server.controller = this;
            cartas = new ModelCartas();

        }
        public void inicio()
        {

            apuestaMinima = 50;

            Thread h1 = new Thread(new ThreadStart(mutexH1));
            Thread h2 = new Thread(new ThreadStart(mutexH2));
            Thread h3 = new Thread(new ThreadStart(mutexH3));
            Thread h4 = new Thread(new ThreadStart(mutexH4));

            h1.Start();
            //h1.Join();

            h2.Start();
            //h2.Join();

            h3.Start();
            //h3.Join();

            h4.Start();
            //h4.Join();

            this.mutexGeneral();

        }

        public void mutexGeneral()
        {
            bool nuevo_juego = true;//sirve para identificar si va empezar un nuevo juego
            bool vuelta = true;//Sirve solamente para dar permiso que se ejecute el metodo "cobro"
            //y se activa cuando al menos los 4 jugadores ya participaron. Esto sirve por si en una jugada 
            //nadie aposto y la apuesta queda en 0;
            int contHilos = 1;
            int ciega = 0;

            while (true)
            {

                if (nuevo_juego) { this.nuevoJuego(ref nuevo_juego, contHilos); ciega++; if (ciega == 5) { ciega = 1; } }
                if (vuelta)
                {
                    this.cobro(ref nuevo_juego, ref contHilos, ref vuelta);
                    ////////////////prueba//////////
                    string str = "";
                    foreach (Carta c in cartas.getMesa())
                    {
                        str += (c.getNumero() + c.getSimbolo() + " ");
                    }
                    Console.WriteLine("Cartas: " + str);
                    Console.WriteLine("Pozo:" + pozo);
                    Console.WriteLine("apuesta:" + apuesta);
                    ///////////////////////////////

                }

                switch (contHilos)
                {
                    case 1: { hilo1 = true; break; }
                    case 2: { hilo2 = true; break; }
                    case 3: { hilo3 = true; break; }
                    case 4: { hilo4 = true; break; }
                }
                Thread.Sleep(1000);
                Console.ReadLine();
                if (instruccion != "")
                {
                    //////////////////////////////////////

                    //hacer la instruccion
                    if (contHilos == 0) { contHilos = ciega; }
                    contHilos++;
                    if (contHilos == 5) { contHilos = 1; vuelta = true; }

                    instruccion = "a";
                }
            }
        }

        public void nuevoJuego(ref bool nuevoJuego, int jugador)
        {
            switch (jugador)
            {
                case 1:
                    {
                        Console.WriteLine("Ciega cobrada a J1");
                        jugadores[0].setMonto(jugadores[0].getMonto() - apuestaMinima);
                        jugadores[0].setApostado(apuestaMinima);
                        jugadores[1].setMonto(jugadores[1].getMonto() - (apuestaMinima * 2));
                        jugadores[1].setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Ciega cobrada a J2");
                        jugadores[1].setMonto(jugadores[1].getMonto() - apuestaMinima);
                        jugadores[1].setApostado(apuestaMinima);
                        jugadores[2].setMonto(jugadores[2].getMonto() - (apuestaMinima * 2));
                        jugadores[2].setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("Ciega cobrada a J3");
                        jugadores[2].setMonto(jugadores[2].getMonto() - apuestaMinima);
                        jugadores[2].setApostado(apuestaMinima);
                        jugadores[3].setMonto(jugadores[3].getMonto() - (apuestaMinima * 2));
                        jugadores[3].setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("Ciega cobrada a J4");
                        jugadores[3].setMonto(jugadores[3].getMonto() - apuestaMinima);
                        jugadores[3].setApostado(apuestaMinima);
                        jugadores[0].setMonto(jugadores[0].getMonto() - (apuestaMinima * 2));
                        jugadores[0].setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
            }
            apuesta = apuestaMinima * 2;
            this.cartas.vaciarMesa();
            this.repartirCartas();
            nuevoJuego = false;
        }
        public void repartirCartas()
        {
            List<Carta> l = cartas.getCartas();
            cartas.clone(cartas.getCartasOficiales(), ref l);
            cartas.desordenar(cartas.getCartas());

            foreach (Jugador jugador in jugadores)
            {
                jugador.setCarta1(cartas.getCartas()[0]);
                cartas.getCartas().Remove(cartas.getCartas()[0]);
                jugador.setCarta2(cartas.getCartas()[0]);
                cartas.getCartas().Remove(cartas.getCartas()[0]);
            }

        }

        public void cobro(ref bool nuevo_juego, ref int contHilos, ref bool vuelta)
        {
            if (verificarCobro())
            {
                vuelta = false;
                apuesta = 0;
                foreach (Jugador jugador in jugadores)
                {
                    jugador.setApostado(0);
                    jugador.setJugando(true);
                }
                //contHilos = 1;
                if (cartas.getMesa().Count() == 5)
                {
                    nuevo_juego = true;
                    //DEFINIR GANADOR
                    /////////////prueba////////////
                    jugadores[0].setMonto(jugadores[0].getMonto() + pozo);
                    //////////////////////////////////
                    pozo = 0;
                    contHilos = 0;
                }
                else
                {
                    this.cartas.dealing();
                }


            }
        }
        public bool verificarCobro()
        {
            foreach (Jugador jugador in jugadores)
            {
                if (jugador.getApostado() != apuesta && jugador.getJugando() == true) { return false; }
            }
            return true;
        }
        public void mutexH1()
        {
            while (true)
            {
                if (hilo1)
                {
                    server.ReceiveRequests(jugadores[0].Client);
                    Console.WriteLine("Hilo 1 hablando ...");

                    ///esto es mientras, es para una prueba
                    jugadores[0].setApostado(jugadores[0].getApostado() + 50);
                    jugadores[0].setMonto(jugadores[0].getMonto() - 50);
                    pozo += 50;
                    if (apuesta < jugadores[0].getApostado())
                    {
                        int x = jugadores[0].getApostado() - apuesta;
                        apuesta += x;
                    }
                    //////////////////////////////////////

                    ///////////////////////////////
                    Console.WriteLine("J1 apostado: " + jugadores[0].getApostado());
                    Console.WriteLine("Monto de j1: " + jugadores[0].getMonto());
                    ////////////////////////////////

                    //Thread.Sleep(1000);
                    instruccion = "hols";
                    jugadores[0].setJugando(true);
                    hilo1 = false;
                }
            }
        }
        public void mutexH2()
        {

            while (true)
            {
                if (hilo2)
                {

                    Console.WriteLine("Hilo 2 hablando ...");
                    ///////////////////////////////
                    Console.WriteLine("J2 apostado: " + jugadores[1].getApostado());
                    Console.WriteLine("Monto de j2: " + jugadores[1].getMonto());
                    ////////////////////////////////
                    //Thread.Sleep(1000);
                    instruccion = "NO";
                    jugadores[1].setJugando(false);
                    hilo2 = false;
                }
            }
        }
        public void mutexH3()
        {
            while (true)
            {
                if (hilo3)
                {
                    Console.WriteLine("Hilo 3 hablando ...");
                    ///////////////////////////////
                    Console.WriteLine("J3 apostado: " + jugadores[2].getApostado());
                    Console.WriteLine("Monto de j3: " + jugadores[2].getMonto());
                    ////////////////////////////////
                    //Thread.Sleep(1000);
                    instruccion = "NO";
                    jugadores[2].setJugando(false);
                    hilo3 = false;
                }

            }
        }

        public void mutexH4()
        {
            while (true)
            {
                if (hilo4)
                {
                    Console.WriteLine("Hilo 4 hablando ...");
                    ///////////////////////////////
                    Console.WriteLine("J4 apostado: " + jugadores[3].getApostado());
                    Console.WriteLine("Monto de j4: " + jugadores[3].getMonto());
                    ////////////////////////////////
                    //Thread.Sleep(1000);
                    instruccion = "NO";
                    jugadores[3].setJugando(false);
                    hilo4 = false;

                }
            }
        }

        public void apostar(string nombreJugador, string instruccion, int raise)
        {
            /*if(instruccion == "Apostar")
            {
                j.setApostado(j.getApostado() + raise);
                j.setMonto(j.getMonto() - raise);
                pozo += raise;
                if (apuesta < j.getApostado())
                {
                    int x = j.getApostado() - apuesta;
                    apuesta += x;
                }
            }
            if(instruccion == "Igualar")
            {
                int aux = apuesta - j.getApostado();
                j.setApostado(j.getApostado() + aux);
                pozo += aux;
            }
            if (instruccion == "Botar")
            {
                j.setJugando(false);
            }
            */
        }
    }
}
