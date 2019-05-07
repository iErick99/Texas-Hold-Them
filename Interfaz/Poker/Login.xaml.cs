using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.Windows.Shapes;

namespace Poker {

    public partial class Login : Window {

        Cuenta cuenta;
        Mesa mesa;
        Cliente cliente;
        dynamic jugador;

        public Login() {
            InitializeComponent();
            cuenta = null;
            mesa = null;
            jugador = new ExpandoObject();
        }

        private void Brd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void Btn_cerrar_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Hl_registrate_Click(object sender, RoutedEventArgs e) {
            if (cuenta == null) {
                cuenta = new Cuenta();
            }

            this.Hide();
            cuenta.Show();
        }

        //private void Psw_contrasena_KeyUp(object sender, KeyEventArgs e) {
        //    if (e.Key == System.Windows.Input.Key.Enter) {
        //        if (this.psw_contrasena.Password.Equals("001")) {
        //            mesa = new Mesa();
        //            MessageBox.Show("Inicio Correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //            this.Hide();
        //            mesa.Show();
        //        }

        //        else {
        //            MessageBox.Show("Inicio incorrectamente", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
        //            this.txt_usuario.Text = "";
        //            this.psw_contrasena.Password = "";
        //            this.txt_usuario.Focus();
        //        }
        //    }
        //}

        private void Btn_iniciar_Click(object sender, RoutedEventArgs e) {
            jugador.method = "login";
            jugador.usuario = this.txt_usuario.Text;
            jugador.password = this.psw_contrasena.Password;

            string result = JsonConvert.SerializeObject(jugador);

            mesa = new Mesa();
            MessageBox.Show("Inicio Correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Hide();
            mesa.Show();  
        }

        private void Window_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                jugador.method = "login";
                jugador.usuario = this.txt_usuario.Text;
                jugador.password = this.psw_contrasena.Password;

                string result = JsonConvert.SerializeObject(jugador);

                mesa = new Mesa();
                MessageBox.Show("Inicio Correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
                mesa.Show();
            }
        }
    }
}
