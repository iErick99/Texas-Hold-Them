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

        private bool fuera;
        private SoundPlayer player;
        private DispatcherTimer Timer;

        public Mesa() {
            InitializeComponent();
            this.fuera = false;
            player = new SoundPlayer();
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Start();
        }

        private void Repartir() {
            this.Rtg_carta1_jugador1.Visibility = Visibility.Hidden;
            this.Rtg_carta2_jugador1.Visibility = Visibility.Hidden;

            this.Rtg_carta1_jugador2.Visibility = Visibility.Hidden;
            this.Rtg_carta2_jugador2.Visibility = Visibility.Hidden;

            this.Rtg_carta1_jugador3.Visibility = Visibility.Hidden;
            this.Rtg_carta2_jugador3.Visibility = Visibility.Hidden;

            this.Rtg_carta1_jugador4.Visibility = Visibility.Hidden;
            this.Rtg_carta2_jugador4.Visibility = Visibility.Hidden;

            Esperar(2000);
            this.Rtg_carta1_jugador1.Visibility = Visibility.Visible;

            Esperar(2000);
            this.Rtg_carta1_jugador2.Visibility = Visibility.Visible;

            Esperar(2000);
            this.Rtg_carta1_jugador3.Visibility = Visibility.Visible;

            Esperar(2000);
            this.Rtg_carta1_jugador4.Visibility = Visibility.Visible;

            Esperar(2000);
            this.Rtg_carta2_jugador1.Visibility = Visibility.Visible;

            Esperar(2000);
            this.Rtg_carta2_jugador2.Visibility = Visibility.Visible;

            Esperar(2000);
            this.Rtg_carta2_jugador3.Visibility = Visibility.Visible;

            Esperar(2000);
            this.Rtg_carta2_jugador4.Visibility = Visibility.Visible;
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
                this.Rtg_carta1_jugador3.Visibility = Visibility.Hidden;
                this.Rtg_carta2_jugador3.Visibility = Visibility.Hidden;
                player.SoundLocation = "C:\\Users\\alefa\\Documents\\Visual Studio 2019\\Projects\\Texas-Hold-Them\\Interfaz\\Poker\\Sounds\\fold.wav";
                player.Play();
                this.fuera = true;
            }
        }

        private void Elp_jugador3_MouseEnter(object sender, MouseEventArgs e) {
            if (this.fuera) {
                this.Rtg_carta1_jugador3.Opacity = 0.60;
                this.Rtg_carta2_jugador3.Opacity = 0.60;
                this.Rtg_carta1_jugador3.Visibility = Visibility.Visible;
                this.Rtg_carta2_jugador3.Visibility = Visibility.Visible;
            }
        }

        private void Elp_jugador3_MouseLeave(object sender, MouseEventArgs e) {
            if (this.fuera) {
                this.Rtg_carta1_jugador3.Visibility = Visibility.Hidden;
                this.Rtg_carta2_jugador3.Visibility = Visibility.Hidden;
            }
        }

        private void Sp_jugador3_MouseEnter(object sender, MouseEventArgs e) {
            this.Elp_jugador3_MouseEnter(sender, e);
        }

        private void Sp_jugador3_MouseLeave(object sender, MouseEventArgs e) {
            this.Elp_jugador3_MouseLeave(sender, e);
        }

        private async void Esperar(int ms) {
            await Task.Delay(ms);
        }

        private void Timer_Tick(object sender, EventArgs e) {
            if (this.Pgb_tiempo.Value > 0) {
                this.Pgb_tiempo.Value -= 1;
            }
        }

        private void Btn_raise_Click(object sender, RoutedEventArgs e) {
            player.SoundLocation = "C:\\Users\\alefa\\Documents\\Visual Studio 2019\\Projects\\Texas-Hold-Them\\Interfaz\\Poker\\Sounds\\bet-4.wav";
            player.Play();
        }

        private void Btn_pass_Click(object sender, RoutedEventArgs e) {
            
        }

        private void Pgb_tiempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (this.Pgb_tiempo.Value == 5) {
                player.SoundLocation = "C:\\Users\\alefa\\Documents\\Visual Studio 2019\\Projects\\Texas-Hold-Them\\Interfaz\\Poker\\Sounds\\alert-5.wav";
                player.Play();
            }

            else if (this.Pgb_tiempo.Value == 0) {
                this.Btn_fold_Click(sender, e);
            }
        }
    }
}
