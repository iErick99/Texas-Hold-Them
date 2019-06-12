using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor 
{
    class ModelCartas
    {
        private const int numCartas = 52;
        private int tam;
        private List<Carta> cartas;
        private List<Carta> disponibles;
        private List<Carta> mesa;
        public List<Carta> getCartasOficiales() { return cartas; }
        public List<Carta> getMesa() { return mesa; }
        public List<Carta> Mesa
        {
            get { return mesa; }
            set { mesa = value; }
        }
        public List<Carta> Disponibles
        {
            get { return disponibles; }
            set { disponibles = value; }
        }
        public ModelCartas()
        {
            cartas = new List<Carta>();
            disponibles = new List<Carta>();
            mesa = new List<Carta>();
            tam = 52;


            int contSim = 0;
            int contNum = 0;
            int contCar = 0;
            while (contCar < numCartas)
            {
                Carta c = new Carta();
                cartas.Add(c);
                switch (contSim)
                {
                    case 0: { cartas[contCar].setSimbolo("Corazones"); break; }
                    case 1: { cartas[contCar].setSimbolo("Espadas"); break; }
                    case 2: { cartas[contCar].setSimbolo("Diamantes"); break; }
                    case 3: { cartas[contCar].setSimbolo("Treboles"); break; }
                }

                switch (contNum)
                {
                    case 0: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 1: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 2: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 3: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 4: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 5: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 6: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 7: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 8: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 9: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 10: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 11: { cartas[contCar].setNumero(contNum + 2); break; }
                    case 12: { cartas[contCar].setNumero(contNum + 2); break; }
                }

                contCar++;
                contNum++;
                if (contNum == 13) { contSim++; contNum = 0; }


            }
            this.Clone();
            this.Desordenar();
        }
        public void Clone()
        {
            disponibles = new List<Carta>();
            foreach (Carta c in cartas)
            {
                disponibles.Add(c);
            }
        }
        public void Desordenar()
        {
            Carta aux;
            int x = 0, y = 0;
            Random ram = new Random();
            for (int i = 0; i < disponibles.Count() / 2; i++)
            {
                x = ram.Next(0, disponibles.Count());
                y = ram.Next(0, disponibles.Count());

                aux = disponibles[x];
                disponibles[x] = disponibles[y];
                disponibles[y] = aux;
            }

        }
        public void Dealing()
        {
            if (this.mesa.Count() == 0)
            {
                this.mesa.Add(this.disponibles[0]);
                this.disponibles.Remove(this.disponibles[0]);
                this.mesa.Add(this.disponibles[0]);
                this.disponibles.Remove(this.disponibles[0]);
                this.mesa.Add(this.disponibles[0]);
                this.disponibles.Remove(this.disponibles[0]);

            }
            else
            {
                this.mesa.Add(this.disponibles[0]);
                this.disponibles.Remove(this.disponibles[0]);

            }
        }
        public void vaciarMesa()
        {
            this.mesa = new List<Carta>();
        }
    }
}
