using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor {
    class EvaluadorMano {
        private static double valorMano;

        enum Manos {
            kicker = 1,
            pareja = 2,
            doblePareja = 3,
            trio = 4,
            escalera = 5,
            color = 6,
            fullHouse = 7,
            poker = 8,
            escaleraColor = 9,
            escaleraReal = 10
        };

        public static void escaleraReal(List<Carta> cartas) {
            if (cartas[6].getNumero() == 14 && cartas[5].getNumero() == 13 && cartas[4].getNumero() == 12 && cartas[3].getNumero() == 11 && cartas[2].getNumero() == 10) {
                if (cartas[6].getSimbolo().Equals(cartas[5].getSimbolo()) && cartas[5].getSimbolo().Equals(cartas[4].getSimbolo()) && cartas[4].getSimbolo().Equals(cartas[3].getSimbolo()) && cartas[3].getSimbolo().Equals(cartas[2].getSimbolo())) {
                    valorMano = (double)Manos.escaleraReal;
                    return;
                }
            }

            valorMano = 0;
        }

        public static void escaleraColor(List<Carta> cartas) {
            if (cartas[6].getNumero() - 1 == cartas[5].getNumero() && cartas[5].getNumero() - 1 == cartas[4].getNumero() && cartas[4].getNumero() - 1 == cartas[3].getNumero() && cartas[3].getNumero() - 1 == cartas[2].getNumero()) {
                if (cartas[6].getSimbolo().Equals(cartas[5].getSimbolo()) && cartas[5].getSimbolo().Equals(cartas[4].getSimbolo()) && cartas[4].getSimbolo().Equals(cartas[3].getSimbolo()) && cartas[3].getSimbolo().Equals(cartas[2].getSimbolo())) {
                    valorMano = (double)Manos.escaleraColor + (cartas[6].getNumero() / 100.0);
                    return;
                }
            }

            if (cartas[5].getNumero() - 1 == cartas[4].getNumero() && cartas[4].getNumero() - 1 == cartas[3].getNumero() && cartas[3].getNumero() - 1 == cartas[2].getNumero() && cartas[2].getNumero() - 1 == cartas[1].getNumero()) {
                if (cartas[5].getSimbolo().Equals(cartas[4].getSimbolo()) && cartas[4].getSimbolo().Equals(cartas[3].getSimbolo()) && cartas[3].getSimbolo().Equals(cartas[2].getSimbolo()) && cartas[2].getSimbolo().Equals(cartas[1].getSimbolo())) {
                    valorMano = (double)Manos.escaleraColor + (cartas[5].getNumero() / 100.0);
                    return;
                }
            }

            if (cartas[4].getNumero() - 1 == cartas[3].getNumero() && cartas[3].getNumero() - 1 == cartas[2].getNumero() && cartas[2].getNumero() - 1 == cartas[1].getNumero() && cartas[1].getNumero() - 1 == cartas[0].getNumero()) {
                if (cartas[4].getSimbolo().Equals(cartas[3].getSimbolo()) && cartas[3].getSimbolo().Equals(cartas[2].getSimbolo()) && cartas[2].getSimbolo().Equals(cartas[1].getSimbolo()) && cartas[1].getSimbolo().Equals(cartas[0].getSimbolo())) {
                    valorMano = (double)Manos.escaleraColor + (cartas[4].getNumero() / 100.0);
                    return;
                }
            }


            valorMano = 0;
        }

        public static void poker(List<Carta> cartas) {
            if (cartas[0].getNumero() == cartas[1].getNumero() && cartas[1].getNumero() == cartas[2].getNumero() && cartas[2].getNumero() == cartas[3].getNumero()) {
                valorMano = (double)Manos.poker + (cartas[0].getNumero() / 100.0);
                return;
            }

            if (cartas[1].getNumero() == cartas[2].getNumero() && cartas[2].getNumero() == cartas[3].getNumero() && cartas[3].getNumero() == cartas[4].getNumero()) {
                valorMano = (double)Manos.poker + (cartas[1].getNumero() / 100.0);
                return;
            }

            if (cartas[2].getNumero() == cartas[3].getNumero() && cartas[3].getNumero() == cartas[4].getNumero() && cartas[4].getNumero() == cartas[5].getNumero()) {
                valorMano = (double)Manos.poker + (cartas[2].getNumero() / 100.0);
                return;
            }

            if (cartas[3].getNumero() == cartas[4].getNumero() && cartas[4].getNumero() == cartas[5].getNumero() && cartas[5].getNumero() == cartas[6].getNumero()) {
                valorMano = (double)Manos.poker + (cartas[3].getNumero() / 100.0);
                return;
            }

            valorMano = 0;
        }

        public static void fullHouse(List<Carta> cartas) {
            if (cartas[6].getNumero() == cartas[5].getNumero()) {
                for (int i = 4; i > 1; i--) {
                    if (cartas[i].getNumero() == cartas[i - 1].getNumero() && cartas[i].getNumero() == cartas[i - 2].getNumero()) {
                        valorMano = (double)Manos.fullHouse + (cartas[i].getNumero() / 100.0);
                        return;
                    }
                }
            }

            if (cartas[5].getNumero() == cartas[4].getNumero()) {
                for (int i = 3; i > 1; i--) {
                    if (cartas[i].getNumero() == cartas[i - 1].getNumero() && cartas[i].getNumero() == cartas[i - 2].getNumero()) {
                        valorMano = (double)Manos.fullHouse + (cartas[i].getNumero() / 100.0);
                        return;
                    }
                }
            }

            if (cartas[4].getNumero() == cartas[3].getNumero() && (cartas[2].getNumero() == cartas[1].getNumero() && cartas[2].getNumero() == cartas[0].getNumero())) {
                valorMano = (double)Manos.fullHouse + (cartas[2].getNumero() / 100.0);
                return;
            }

            if (cartas[6].getNumero() == cartas[5].getNumero() && cartas[6].getNumero() == cartas[4].getNumero()) {
                for (int i = 3; i > 0; i--) {
                    if (cartas[i].getNumero() == cartas[i - 1].getNumero()) {
                        valorMano = (double)Manos.fullHouse + (cartas[6].getNumero() / 100.0);
                        return;
                    }
                }
            }

            if (cartas[5].getNumero() == cartas[4].getNumero() && cartas[5].getNumero() == cartas[3].getNumero()) {
                for (int i = 2; i > 0; i--) {
                    if (cartas[i].getNumero() == cartas[i - 1].getNumero()) {
                        valorMano = (double)Manos.fullHouse + (cartas[5].getNumero() / 100.0);
                        return;
                    }
                }
            }

            if ((cartas[4].getNumero() == cartas[3].getNumero() && cartas[4].getNumero() == cartas[2].getNumero()) && cartas[1].getNumero() == cartas[0].getNumero()) {
                valorMano = (double)Manos.fullHouse + (cartas[4].getNumero() / 100.0);
                return;
            }

            valorMano = 0;
        }

        public static void color(List<Carta> cartas) {
            try {
                cartas = cartas.OrderBy(o => o.getSimbolo()).ToList();

                if (cartas[6].getSimbolo().Equals(cartas[5].getSimbolo()) && cartas[5].getSimbolo().Equals(cartas[4].getSimbolo()) && cartas[4].getSimbolo().Equals(cartas[3].getSimbolo()) && cartas[3].getSimbolo().Equals(cartas[2].getSimbolo())) {
                    valorMano = (double)Manos.color + (cartas[6].getNumero() / 100.0);
                    return;
                }

                if (cartas[5].getSimbolo().Equals(cartas[4].getSimbolo()) && cartas[4].getSimbolo().Equals(cartas[3].getSimbolo()) && cartas[3].getSimbolo().Equals(cartas[2].getSimbolo()) && cartas[2].getSimbolo().Equals(cartas[1].getSimbolo())) {
                    valorMano = (double)Manos.color + (cartas[5].getNumero() / 100.0);
                    return;
                }

                if (cartas[4].getSimbolo().Equals(cartas[3].getSimbolo()) && cartas[3].getSimbolo().Equals(cartas[2].getSimbolo()) && cartas[2].getSimbolo().Equals(cartas[1].getSimbolo()) && cartas[1].getSimbolo().Equals(cartas[0].getSimbolo())) {
                    valorMano = (double)Manos.color + (cartas[4].getNumero() / 100.0);
                    return;
                }

                valorMano = 0;
            }
            finally {
                cartas = cartas.OrderBy(o => o.getNumero()).ToList();
            }
        }

        public static void escalera(List<Carta> cartas) {
            if (cartas[6].getNumero() - 1 == cartas[5].getNumero() && cartas[5].getNumero() - 1 == cartas[4].getNumero() && cartas[4].getNumero() - 1 == cartas[3].getNumero() && cartas[3].getNumero() - 1 == cartas[2].getNumero()) {
                valorMano = (double)Manos.escalera + (cartas[6].getNumero() / 100.0);
                return;
            }

            if (cartas[5].getNumero() - 1 == cartas[4].getNumero() && cartas[4].getNumero() - 1 == cartas[3].getNumero() && cartas[3].getNumero() - 1 == cartas[2].getNumero() && cartas[2].getNumero() - 1 == cartas[1].getNumero()) {
                valorMano = (double)Manos.escalera + (cartas[5].getNumero() / 100.0);
                return;
            }

            if (cartas[4].getNumero() - 1 == cartas[3].getNumero() && cartas[3].getNumero() - 1 == cartas[2].getNumero() && cartas[2].getNumero() - 1 == cartas[1].getNumero() && cartas[1].getNumero() - 1 == cartas[0].getNumero()) {
                valorMano = (double)Manos.escalera + (cartas[4].getNumero() / 100.0);
                return;
            }
            
            valorMano = 0;
        }

        public static void trio(List<Carta> cartas) {
            if (cartas[6].getNumero() == cartas[5].getNumero() && cartas[5].getNumero() == cartas[4].getNumero()) {
                valorMano = (double)Manos.trio + (cartas[6].getNumero() / 100.0);
                return;
            }

            if (cartas[5].getNumero() == cartas[4].getNumero() && cartas[4].getNumero() == cartas[3].getNumero()) {
                valorMano = (double)Manos.trio + (cartas[5].getNumero() / 100.0);
                return;
            }

            if (cartas[4].getNumero() == cartas[3].getNumero() && cartas[3].getNumero() == cartas[2].getNumero()) { 
                valorMano = (double)Manos.trio + (cartas[4].getNumero() / 100.0);
                return;
            }

            if (cartas[3].getNumero() == cartas[2].getNumero() && cartas[2].getNumero() == cartas[1].getNumero()) {
                valorMano = (double)Manos.trio + (cartas[3].getNumero() / 100.0);
                return;
            }

            if (cartas[2].getNumero() == cartas[1].getNumero() && cartas[1].getNumero() == cartas[0].getNumero()) {
                valorMano = (double)Manos.trio + (cartas[2].getNumero() / 100.0);
                return;
            }

            valorMano = 0;
        }

        public static void doblePareja(List<Carta> cartas) {
            if (cartas[6].getNumero() == cartas[5].getNumero()) {
                for (int i = 4; i > 0; i--) {
                    if (cartas[i].getNumero() == cartas[i - 1].getNumero()) {
                        valorMano = (double)Manos.doblePareja + (cartas[6].getNumero() / 100.0);
                        return;
                    }
                }
            }

            if (cartas[5].getNumero() == cartas[4].getNumero()) {
                for (int i = 3; i > 0; i--) {
                    if (cartas[i].getNumero() == cartas[i - 1].getNumero()) {
                        valorMano = (double)Manos.doblePareja + (cartas[5].getNumero() / 100.0);
                        return;
                    }
                }
            }

            if (cartas[4].getNumero() == cartas[3].getNumero()) {
                for (int i = 2; i > 0; i--) {
                    if (cartas[i].getNumero() == cartas[i - 1].getNumero()) {
                        valorMano = (double)Manos.doblePareja + (cartas[4].getNumero() / 100.0);
                        return;
                    }
                }
            }

            if (cartas[3].getNumero() == cartas[2].getNumero() && cartas[1].getNumero() == cartas[0].getNumero()) {
                valorMano = (double)Manos.doblePareja + (cartas[3].getNumero() / 100.0);
                return;
            }

            valorMano = 0;
        }

        public static void pareja(List<Carta> cartas) {
            for (int i = 6; i > 0; i--) {
                if (cartas[i].getNumero() == cartas[i - 1].getNumero()) {
                    valorMano = (double)Manos.pareja + (cartas[i].getNumero() / 100.0);
                    return;
                }
            }

            valorMano = 0;
        }

        //Usar en caso de empate
        public static double kicker(List<Carta> cartas) {
            return (double)Manos.kicker + cartas[6].getNumero() / 100.0;
        }

        //Usar en caso de que kicker() no haya desempatado la jugada
        public static double kicker2(List<Carta> cartas) {
            return (double)Manos.kicker + cartas[5].getNumero() / 100.0;
        }

        //Usar en caso de que kicker2() no haya desempatado la jugada
        public static double kicker3(List<Carta> cartas) {
            return (double)Manos.kicker + cartas[4].getNumero() / 100.0;
        }

        //Usar en caso de que kicker3() no haya desempatado la jugada
        public static double kicker4(List<Carta> cartas) {
            return (double)Manos.kicker + cartas[3].getNumero() / 100.0;
        }

        //Usar en caso de que kicker4() no haya desempatado la jugada
        public static double kicker5(List<Carta> cartas) {
            return (double)Manos.kicker + cartas[2].getNumero() / 100.0;
        }

        public static double EvaluarMano(List<Carta> cartas) {
            valorMano = 0;
            cartas = cartas.OrderBy(o => o.getNumero()).ToList();

            escaleraReal(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            escaleraColor(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            poker(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            fullHouse(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            color(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            escalera(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            trio(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            doblePareja(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            pareja(cartas);
            if (valorMano != 0) {
                return valorMano;
            }

            return kicker(cartas);
        }
    }
}
