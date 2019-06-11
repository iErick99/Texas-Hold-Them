﻿using System;
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
        private ModelCartas cartas;
        private List<Jugador> jugadores = new List<Jugador>();
        private String turno = "";
        

        public bool nuevo_juego = true;//sirve para identificar si va empezar un nuevo juego

        public bool vuelta = true;//Sirve solamente para dar permiso que se ejecute el metodo "cobro"
                                  //y se activa cuando al menos los 4 jugadores ya participaron. Esto sirve por si en una jugada 
                                  //nadie aposto y la apuesta queda en 0;
        public int contHilos = 1;
        public int ciega = 1;
        private int contVueltas=0;
        private int limVueltas = 4;

        public string dealer = "";
        public Controller()
        {
            cartas = new ModelCartas();
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

            apuestaMinima = 25;

            if (nuevo_juego) { this.nuevoJuego(ref nuevo_juego, contHilos); ciega++; if (ciega == 5) { ciega = 1; } }
            if (vuelta){ this.cobro(ref nuevo_juego, ref contHilos, ref vuelta); }
            switch (contHilos)
            {
                case 1: { turno = jugadores[0].Nombre; break; }
                case 2: { turno = jugadores[1].Nombre; break; }
                case 3: { turno = jugadores[2].Nombre; break; }
                case 4: { turno = jugadores[3].Nombre; break; }
            }
            if (contHilos == 0) { contHilos = ciega; }
            if (contHilos == 5) { contHilos = 1; vuelta = true; }

        }

        public void nuevoJuego(ref bool nuevoJuego, int jugador)
        {
            foreach (Jugador j in jugadores)
            {
                j.setApostado(0);
                j.setJugando(true);
                if (j.getMonto() < apuestaMinima)
                {
                    j.setMonto(0);
                    j.setJugando(false);
                    j.setCarta1(null);
                    j.setCarta2(null);
                    limVueltas--;
                    jugador++;
                    if (jugador == 5) jugador = 1;
                    if (jugador == 6) jugador = 2;
                    contHilos=jugador;
                }
            }
            limVueltas = 4;
            this.calcularDealer(jugador);
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
            contHilos += 2;
            if (contHilos == 5) contHilos = 1;
            if (contHilos == 6) contHilos = 2;
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
                contVueltas = 0;
                vuelta = false;
                apuesta = 0;
                foreach (Jugador jugador in jugadores)
                {
                    jugador.setApostado(0);
                }
                if (cartas.getMesa().Count() == 5)
                {
                    nuevo_juego = true;
                    //DEFINIR GANADOR
                    this.escogerGanador();
                    //////////////////////
                    pozo = 0;
                    apuesta = 0;
                    this.cartas.vaciarMesa();
                    contHilos = ciega;
                    this.nuevoJuego(ref nuevo_juego, contHilos);
                    ciega++;
                    if (ciega == 5) { ciega = 1; apuestaMinima = apuestaMinima * 2; }
                    contHilos--;
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
                if (jugador.getApostado() != apuesta && jugador.getJugando() == true ) { return false; }
            }
            return true;
        }
        

        public void apostar(string nombreJugador, string instruccion, int raise)
        {
            Jugador j = BuscarJugador(nombreJugador);

            if (j.getMonto() <= 0)
            {
                j.setMonto(0);
                j.setJugando(false);
                j.setCarta1(null);
                j.setCarta2(null);
                limVueltas--;
                instruccion = "pasar";
            }

            if (instruccion == "Apostar")
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
                    int aux = apuesta - j.getApostado();
                    j.setApostado(j.getMonto());
                    pozo += aux;
                    j.setMonto(0);
                    j.setJugando(false);
                    limVueltas--;
                }
            }
            if (instruccion == "Botar")
            {
                j.setJugando(false);
                j.setCarta1(null);
                j.setCarta2(null);
                limVueltas--;
            }
            this.Procesar();
        }

        public void Procesar()
        {
            if (vuelta) { this.cobro(ref nuevo_juego, ref contHilos, ref vuelta); }
            contHilos++;
            contVueltas++;
            if (contHilos == 5) { contHilos = 1; }
            if (contVueltas >= limVueltas) { vuelta = true; }
            turno = asignarTurno();
        }

        public string asignarTurno()
        {
            while (true)
            {
                if (contHilos == 1 && jugadores[0].getJugando() == true) { return jugadores[0].Nombre; }
                if (contHilos == 2 && jugadores[1].getJugando() == true) { return jugadores[1].Nombre; }
                if (contHilos == 3 && jugadores[2].getJugando() == true) { return jugadores[2].Nombre; }
                if (contHilos == 4 && jugadores[3].getJugando() == true) { return jugadores[3].Nombre; }
                contHilos++;
                if (contHilos == 5) { contHilos = 1; }
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

        

        public void escogerGanador()
        {
            jugadores[0].setValorMano(0);
            jugadores[1].setValorMano(0);
            jugadores[2].setValorMano(0);
            jugadores[3].setValorMano(0);

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
                Console.WriteLine("Ganador: Jugador 0 con: " + jugadores[0].getValorMano());
                jugadores[0].setMonto(jugadores[0].getMonto() + pozo);
            }

            else if (jugadores[1].getValorMano() > jugadores[0].getValorMano() && jugadores[1].getValorMano() > jugadores[2].getValorMano() && jugadores[1].getValorMano() > jugadores[3].getValorMano())
            {
                Console.WriteLine("Ganador: Jugador 1 con: " + jugadores[1].getValorMano());
                jugadores[1].setMonto(jugadores[1].getMonto() + pozo);
            }

            else if (jugadores[2].getValorMano() > jugadores[0].getValorMano() && jugadores[2].getValorMano() > jugadores[1].getValorMano() && jugadores[2].getValorMano() > jugadores[3].getValorMano())
            {
                Console.WriteLine("Ganador: Jugador 2 con: " + jugadores[2].getValorMano());
                jugadores[2].setMonto(jugadores[2].getMonto() + pozo);
            }

            else if (jugadores[3].getValorMano() > jugadores[0].getValorMano() && jugadores[3].getValorMano() > jugadores[1].getValorMano() && jugadores[3].getValorMano() > jugadores[2].getValorMano())
            {
                Console.WriteLine("Ganador: Jugador 3 con: " + jugadores[3].getValorMano());
                jugadores[3].setMonto(jugadores[3].getMonto() + pozo);
            }
        }

        public void calcularDealer(int jugador)
        {
            if (jugador == 1) dealer = jugadores[3].Nombre;
            if (jugador == 2) dealer = jugadores[0].Nombre;
            if (jugador == 3) dealer = jugadores[1].Nombre;
            if (jugador == 4) dealer = jugadores[2].Nombre;
        }

    }
}
