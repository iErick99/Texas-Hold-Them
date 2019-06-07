using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor {
    class Dealer {

        private List<Carta> mazo;

        public Dealer() {
            this.mazo = new List<Carta>();
            this.llenarMazo();
        }

        public void llenarMazo() {
            for (int i = 0; i < 4; i++) {
                int num = 2;

                for (int j = 0; j < 13; j++) {
                    if (i == 0) {
                        mazo.Add(new Carta(num, "Treboles"));
                        num++;
                    }

                    else if (i == 1) {
                        mazo.Add(new Carta(num, "Corazones"));
                        num++;
                    }

                    else if (i == 2) {
                        mazo.Add(new Carta(num, "Espadas"));
                        num++;
                    }

                    else {
                        mazo.Add(new Carta(num, "Diamantes"));
                        num++;
                    }
                }
            }
        }

        public List<Carta> getMazo() {
            return this.mazo;
        }
    }
}
