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
        private int pozo = 0;
        private int apuesta = 0;
        private int apuestaMinima = 0;
        private string instruccion = "";
        private ModelCartas cartas;
        private List<Jugador> jugadores = new List<Jugador>();
        private String turno = "";


        public bool nuevo_juego = true;//sirve para identificar si va empezar un nuevo juego
        public bool vuelta = true;//Sirve solamente para dar permiso que se ejecute el metodo "cobro"
                                  //y se activa cuando al menos los 4 jugadores ya participaron. Esto sirve por si en una jugada 
                                  //nadie aposto y la apuesta queda en 0;
        public int contHilos = 1;
        public int ciega = 0;
        public Controller()
        {
            cartas = new ModelCartas();
            //ARRACO EL SERVER
            //llAMO 4 HILOS 
        }
        public int Pozo
        {
            get { return pozo; }
            set { pozo = value; }
        }
        public ModelCartas Cartas
        {
            get { return cartas; }
            set { cartas = value; }
        }
        public List<Jugador> Jugadores
        {
            get { return jugadores; }
            set { jugadores = value; }
        }
        public String Turno
        {
            get { return turno; }
            set { turno = value; }
        }

        public void inicio()
        {

            apuestaMinima = 50;

            this.mutexGeneral();

        }

        public void mutexGeneral()
        {
                if (nuevo_juego) { this.nuevoJuego(ref nuevo_juego, contHilos); ciega++; if (ciega == 5) { ciega = 1; } }
                if (vuelta){ this.cobro(ref nuevo_juego, ref contHilos, ref vuelta); }
                switch (contHilos)
                {
                    case 1: { turno = jugadores[0].Nombre; break; }
                    case 2: { turno = jugadores[1].Nombre; break; }
                    case 3: { turno = jugadores[2].Nombre; break; }
                    case 4: { turno = jugadores[3].Nombre; break; }
                }
                if (instruccion != "")
                {
                    if (contHilos == 0) { contHilos = ciega; }
                    contHilos++;
                    if (contHilos == 5) { contHilos = 1; vuelta = true; }

                    instruccion = "";
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
                if (cartas.getMesa().Count() == 5)
                {
                    nuevo_juego = true;
                    //DEFINIR GANADOR
                    /////////////////////////
                    this.escogerGanador();
                    //////////////////////////////////
                    pozo = 0;
                    apuesta = 0;
                    contHilos = 0;
                    instruccion = "nuevoJuego";
                    this.cartas.vaciarMesa();
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
        

        public void apostar(string nombreJugador, string instruccion, int raise)
        {
            Jugador j = BuscarJugador(nombreJugador);
            if(instruccion == "Apostar")
            {
                j.setApostado(j.getApostado() + raise);
                j.setMonto(j.getMonto() - raise);
                pozo += raise;
                Console.WriteLine(instruccion);
                if (apuesta < j.getApostado())
                {
                    apuesta = j.getApostado();
                }
            }
            if(instruccion == "Igualar")
            {
                if (j.getMonto() > apuesta)
                {
                    int aux = apuesta - j.getApostado();
                    j.setApostado(apuesta);
                    pozo += aux;
                    j.setMonto(j.getMonto() - aux);
                }
                else
                {
                    j.setApostado(j.getMonto());
                    pozo += j.getMonto();
                    j.setMonto(0);
                }
            }
            if (instruccion == "Botar")
            {
                j.setJugando(false);
                j.setCarta1(null);
                j.setCarta2(null);
            }
            instruccion = "sigaRecto";

            if (nuevo_juego) { this.nuevoJuego(ref nuevo_juego, contHilos); ciega++; if (ciega == 5) { ciega = 1; apuestaMinima += 50; } }
            if (vuelta) { this.cobro(ref nuevo_juego, ref contHilos, ref vuelta); }
            if (instruccion != "")
            {
                if (contHilos == 0) { contHilos = ciega; }
                contHilos++;
                if (contHilos == 5) { contHilos = 1; vuelta = true; }

                instruccion = "";
            }
            switch (contHilos)
            {
                case 1: { turno = jugadores[0].Nombre; break; }
                case 2: { turno = jugadores[1].Nombre; break; }
                case 3: { turno = jugadores[2].Nombre; break; }
                case 4: { turno = jugadores[3].Nombre; break; }
            }



        }

        public Jugador BuscarJugador(string nombreJugador)
        {
            if (jugadores[0].Nombre == nombreJugador) return jugadores[0];
            if (jugadores[1].Nombre == nombreJugador) return jugadores[1];
            if (jugadores[2].Nombre == nombreJugador) return jugadores[2];
            if (jugadores[3].Nombre == nombreJugador) return jugadores[3];
            return null;
        }

        public string asignarTurno(int x)
        {
            if (x == 0)
            {
                if (nuevo_juego) { this.nuevoJuego(ref nuevo_juego, contHilos); ciega++; if (ciega == 5) { ciega = 1; apuestaMinima += 50; } }
                x++;
            }
            while (true)
            {
                if (x == 1 && jugadores[0].getJugando() == true) { return jugadores[0].Nombre; }
                if (x == 2 && jugadores[1].getJugando() == true) { return jugadores[1].Nombre; }
                if (x == 3 && jugadores[2].getJugando() == true) { return jugadores[2].Nombre; }
                if (x == 4 && jugadores[3].getJugando() == true) { return jugadores[3].Nombre; }
                x++;
            }
        }
        public void escogerGanador()
        {
            if (jugadores[0].getJugando())
            {
                cartas.getMesa().Add(jugadores[0].getCarta1());
                cartas.getMesa().Add(jugadores[0].getCarta2());
                jugadores[0].setValorMano(EvaluadorMano.EvaluarMano(cartas.getMesa()));
                cartas.getMesa().Remove(jugadores[0].getCarta1());
                cartas.getMesa().Remove(jugadores[0].getCarta2());
            }

            if (jugadores[1].getJugando())
            {
                cartas.getMesa().Add(jugadores[1].getCarta1());
                cartas.getMesa().Add(jugadores[1].getCarta2());
                jugadores[1].setValorMano(EvaluadorMano.EvaluarMano(cartas.getMesa()));
                cartas.getMesa().Remove(jugadores[1].getCarta1());
                cartas.getMesa().Remove(jugadores[1].getCarta2());
            }

            if (jugadores[2].getJugando())
            {
                cartas.getMesa().Add(jugadores[2].getCarta1());
                cartas.getMesa().Add(jugadores[2].getCarta2());
                jugadores[2].setValorMano(EvaluadorMano.EvaluarMano(cartas.getMesa()));
                cartas.getMesa().Remove(jugadores[2].getCarta1());
                cartas.getMesa().Remove(jugadores[2].getCarta2());
            }

            if (jugadores[3].getJugando())
            {
                cartas.getMesa().Add(jugadores[3].getCarta1());
                cartas.getMesa().Add(jugadores[3].getCarta2());
                jugadores[3].setValorMano(EvaluadorMano.EvaluarMano(cartas.getMesa()));
                cartas.getMesa().Remove(jugadores[3].getCarta1());
                cartas.getMesa().Remove(jugadores[3].getCarta2());
            }

            if (jugadores[0].getValorMano() > jugadores[1].getValorMano() && jugadores[0].getValorMano() > jugadores[2].getValorMano() && jugadores[0].getValorMano() > jugadores[3].getValorMano())
            {
                Console.WriteLine("Ganador: Jugador 0");
                jugadores[0].setMonto(jugadores[0].getMonto() + pozo);
            }

            else if (jugadores[1].getValorMano() > jugadores[0].getValorMano() && jugadores[1].getValorMano() > jugadores[2].getValorMano() && jugadores[1].getValorMano() > jugadores[3].getValorMano())
            {
                Console.WriteLine("Ganador: Jugador 1");
                jugadores[1].setMonto(jugadores[1].getMonto() + pozo);
            }

            else if (jugadores[2].getValorMano() > jugadores[0].getValorMano() && jugadores[2].getValorMano() > jugadores[1].getValorMano() && jugadores[2].getValorMano() > jugadores[3].getValorMano())
            {
                Console.WriteLine("Ganador: Jugador 2");
                jugadores[2].setMonto(jugadores[2].getMonto() + pozo);
            }

            else if (jugadores[3].getValorMano() > jugadores[0].getValorMano() && jugadores[3].getValorMano() > jugadores[1].getValorMano() && jugadores[3].getValorMano() > jugadores[2].getValorMano())
            {
                Console.WriteLine("Ganador: Jugador 3");
                jugadores[3].setMonto(jugadores[3].getMonto() + pozo);
            }
        }
    }
}
