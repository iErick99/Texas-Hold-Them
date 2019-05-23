using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Servidor 
{
    class Controller
    {
        public int pozo = 0;
        public int apuesta = 0;

        public int apuestaMinima = 0;

        public Jugador j1;
        public Jugador j2;
        public Jugador j3;
        public Jugador j4;

        public bool hilo1;
        public bool hilo2;
        public bool hilo3;
        public bool hilo4;

        public string instruccion = "";

        public ModelCartas cartas;



        public Controller()
        {
            cartas = new ModelCartas();

        }
        public void inicio()
        {


            apuestaMinima = 50;

            j1 = new Jugador();
            j2 = new Jugador();
            j3 = new Jugador();
            j4 = new Jugador();


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
            

            while (true)
            {
                if (nuevo_juego) { this.nuevoJuego(ref nuevo_juego, contHilos); }
                if (vuelta) { this.cobro(ref nuevo_juego, ref contHilos, ref vuelta); }
                
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
                    contHilos++;
                    if (contHilos == 5) { contHilos = 1; vuelta = true; }
                    instruccion = "";
                }
            }
        }

        public void nuevoJuego(ref bool nuevoJuego, int jugador)
        {
            switch (jugador)
            {
                case 1:
                    {
                        j1.setMonto(j1.getMonto() - apuestaMinima);
                        j1.setApostado(apuestaMinima);
                        j2.setMonto(j2.getMonto() - (apuestaMinima * 2));
                        j2.setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
                case 2:
                    {
                        j2.setMonto(j2.getMonto() - apuestaMinima);
                        j2.setApostado(apuestaMinima);
                        j3.setMonto(j3.getMonto() - (apuestaMinima * 2));
                        j3.setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
                case 3:
                    {
                        j3.setMonto(j3.getMonto() - apuestaMinima);
                        j3.setApostado(apuestaMinima);
                        j4.setMonto(j4.getMonto() - (apuestaMinima * 2));
                        j4.setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
                case 4:
                    {
                        j4.setMonto(j4.getMonto() - apuestaMinima);
                        j4.setApostado(apuestaMinima);
                        j1.setMonto(j1.getMonto() - (apuestaMinima * 2));
                        j1.setApostado(apuestaMinima * 2);
                        pozo = pozo + apuestaMinima + (apuestaMinima * 2);
                        break;
                    }
            }
            apuesta = apuestaMinima * 2;

            this.repartirCartas();
            nuevoJuego = false;
        }
        public void repartirCartas()
        {
            List<Carta> l = cartas.getCartas();
            cartas.clone(cartas.getCartasOficiales(), ref l);
            cartas.desordenar(cartas.getCartas());

            j1.setCarta1(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);
            j2.setCarta1(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);
            j3.setCarta1(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);
            j4.setCarta1(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);

            j1.setCarta2(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);
            j2.setCarta2(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);
            j3.setCarta2(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);
            j4.setCarta2(cartas.getCartas()[0]);
            cartas.getCartas().Remove(cartas.getCartas()[0]);

        }
        public void dealing()
        {
            if (cartas.getMesa().Count() == 0)
            {
                cartas.getMesa().Add(cartas.getCartas()[0]);
                cartas.getCartas().Remove(cartas.getCartas()[0]);
                cartas.getMesa().Add(cartas.getCartas()[0]);
                cartas.getCartas().Remove(cartas.getCartas()[0]);
                cartas.getMesa().Add(cartas.getCartas()[0]);
                cartas.getCartas().Remove(cartas.getCartas()[0]);

            }
            else
            {
                cartas.getMesa().Add(cartas.getCartas()[0]);
                cartas.getCartas().Remove(cartas.getCartas()[0]);

            }
        }
        public void cobro(ref bool nuevo_juego,ref int contHilos, ref bool vuelta)
        {
            if (verificarCobro())
            {
                vuelta = false;
                apuesta = 0;
                j1.setApostado(0);
                j2.setApostado(0);
                j3.setApostado(0);
                j4.setApostado(0);

                j1.setJugando(true);
                j2.setJugando(true);
                j3.setJugando(true);
                j4.setJugando(true);
                contHilos = 1;
                if (cartas.getMesa().Count() == 5)
                {
                    nuevo_juego = true;
                    //DEFINIR GANADOR
                    /////////////prueba////////////
                    j1.setMonto(j1.getMonto() + pozo);
                    //////////////////////////////////
                    pozo = 0;
                    
                }
                else
                {
                    this.dealing();
                }


            }
        }
        public bool verificarCobro()
        {
            if (j1.getApostado() != apuesta && j1.getJugando() == true) { return false; }
            if (j2.getApostado() != apuesta && j2.getJugando() == true) { return false; }
            if (j3.getApostado() != apuesta && j3.getJugando() == true) { return false; }
            if (j4.getApostado() != apuesta && j4.getJugando() == true) { return false; }
            return true;
        }
        public void mutexH1()
        {
            while (true)
            {
                if (hilo1)
                {
                    Console.WriteLine("Hilo 1 hablando ...");
                    ///esto es mientras, es para una prueba
                    j1.setApostado(j1.getApostado() + 50);
                    j1.setMonto(j1.getMonto() - 50);
                    pozo += 50;
                    if (apuesta < j1.getApostado())
                    {
                        int x = j1.getApostado() - apuesta;
                        apuesta += x;
                    }
                    //////////////////////////////////////
                    ////////////////prueba//////////
                    Console.WriteLine("Cartas:");
                    foreach(Carta c in cartas.getMesa())
                    {
                        Console.WriteLine(c.getNumero() + c.getSimbolo());
                    }
                    Console.WriteLine("Pozo:"+pozo);
                    Console.WriteLine("apuesta:"+apuesta);
                    Console.WriteLine("J1 apostado: "+j1.getApostado());
                    Console.WriteLine("Monto de j1: "+j1.getMonto());
                    ////////////////////////////////

                    //Thread.Sleep(1000);
                    instruccion = "hols";
                    j1.setJugando(true);
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
                    //Thread.Sleep(1000);
                    instruccion = "NO";
                    j2.setJugando(false);
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
                    //Thread.Sleep(1000);
                    instruccion = "NO";
                    j3.setJugando(false);
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
                    //Thread.Sleep(1000);
                    instruccion = "NO";
                    j4.setJugando(false);
                    hilo4 = false;

                }
            }
        }

        public void parse(string str)
        {
            Console.WriteLine("Parseando..");
        }
    }
}
