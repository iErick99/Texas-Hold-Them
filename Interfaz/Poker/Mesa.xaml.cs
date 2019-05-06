using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
using System.Windows.Media.Animation;

namespace Poker {
    public partial class Mesa : Window {

        enum Cartas {
            c = 1,
            d = 2,
            h = 3,
            s = 4
        }

        private bool fuera;
        private SoundPlayer Sonido;
        private DispatcherTimer Timer;
        private bool ancho;

        public Mesa() {
            InitializeComponent();
            this.fuera = false;
            Sonido = new SoundPlayer();
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Start();
            ancho = false;
            
        }

        private void EscogerCarta() {
            Random numero = new Random();
            MessageBox.Show(numero.Next(1, 5) + "");

            MessageBox.Show(numero.Next(1, 14) + "");



            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri("/Images/c" + numero.Next(1, 14) + ".png", UriKind.Relative);
            b.EndInit();

        }

        private void Btn_cerrar_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Brd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void Btn_min_Click(object sender, RoutedEventArgs e) {
            this.wnd_ventana.WindowState = WindowState.Minimized;
        }

        private void Sld_apuesta_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            this.lbl_apuesta.Content = (int)this.sld_apuesta.Value;

            if (this.sld_apuesta.Value == 1000) {
                this.lbl_apuesta.Content = "All in";
            }
        }

        private void Btn_restar_Click(object sender, RoutedEventArgs e) {
            this.sld_apuesta.Value -= 1;
        }

        private void Btn_sumar_Click(object sender, RoutedEventArgs e) {
            this.sld_apuesta.Value += 1;
        }

        private void Btn_fold_Click(object sender, RoutedEventArgs e) {
            if (!this.fuera) {
                this.Img_carta1_jugador3.Visibility = Visibility.Hidden;
                this.Img_carta2_jugador3.Visibility = Visibility.Hidden;
                Sonido.SoundLocation = "../../Sounds/fold.wav";
                Sonido.Play();
                this.fuera = true;
            }
        }

        private void Elp_jugador3_MouseEnter(object sender, MouseEventArgs e) {
            
            if (this.fuera) {
                this.Img_carta1_jugador3.Opacity = 0.60;
                this.Img_carta2_jugador3.Opacity = 0.60;
                this.Img_carta1_jugador3.Visibility = Visibility.Visible;
                this.Img_carta2_jugador3.Visibility = Visibility.Visible;
            }
        }

        private void Elp_jugador3_MouseLeave(object sender, MouseEventArgs e) {
            if (this.fuera) {
                this.Img_carta1_jugador3.Visibility = Visibility.Hidden;
                this.Img_carta2_jugador3.Visibility = Visibility.Hidden;
            }
        }

        private void Sp_jugador3_MouseEnter(object sender, MouseEventArgs e) {
            this.Elp_jugador3_MouseEnter(sender, e);
        }

        private void Sp_jugador3_MouseLeave(object sender, MouseEventArgs e) {
            this.Elp_jugador3_MouseLeave(sender, e);
        }

        private void Timer_Tick(object sender, EventArgs e) {
            if (this.Pgb_tiempo.Value > 0 && !fuera) {
                this.Pgb_tiempo.Value -= 1;

                if (!ancho) {
                    this.Elp_jugador3.StrokeThickness = 5;
                    ancho = true;
                }

                else {
                    this.Elp_jugador3.StrokeThickness = 1;
                    ancho = false;
                }
            }

            else {
                this.Elp_jugador3.StrokeThickness = 1;
                ancho = false;
                this.Pgb_tiempo.Value = 15;
            }
        }

        private void Btn_raise_Click(object sender, RoutedEventArgs e) {
            Sonido.SoundLocation = "../../Sounds/bet-4.wav";
            Sonido.Play();
            this.Lbl_apuesta_jugador3.Content = lbl_apuesta.Content;
            this.sld_apuesta.Value = 0;
            fuera = true;
        }

        private void Btn_pass_Click(object sender, RoutedEventArgs e) {
            this.EscogerCarta();
        }

        private void Pgb_tiempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (this.Pgb_tiempo.Value == 5) {
                Sonido.SoundLocation = "../../Sounds/alert-5.wav";
                Sonido.Play();
            }

            else if (this.Pgb_tiempo.Value == 0) {
                this.Btn_fold_Click(sender, e);
            }
        }
    }
}
