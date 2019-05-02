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

namespace Poker {
    public partial class Mesa : Window {

        public bool fuera;
        public Mesa() {
            InitializeComponent();
            this.fuera = false;
        }

        private void Btn_cerrar_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Brd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        //private void Btn_max_Click(object sender, RoutedEventArgs e) {
        //    if (this.wnd_ventana.WindowState == WindowState.Normal) {
        //        this.wnd_ventana.WindowState = WindowState.Maximized;
        //        this.Icn_max.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
        //    }

        //    else {
        //        this.wnd_ventana.WindowState = WindowState.Normal;
        //        this.Icn_max.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
        //    }
        //}

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
    }
}
