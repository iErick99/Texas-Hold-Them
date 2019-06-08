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

namespace Poker
{
    /// <summary>
    /// Lógica de interacción para Cuenta.xaml
    /// </summary>
    public partial class Cuenta : Window
    {

        private dynamic jugador;

        public Cuenta()
        {
            InitializeComponent();
            jugador = new ExpandoObject();
        }

        private void Brd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Btn_cerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Btn_crear_Click(object sender, RoutedEventArgs e)
        {
            if (this.psw_confirmarContrasena.Password.Equals(this.psw_contrasena.Password))
            {
                Client.client.Connect("13.90.205.129", 100);

                jugador.method = "create";
                jugador.name = this.txt_usuario.Text;
                jugador.usuario = this.txt_usuario.Text;
                jugador.password = this.psw_contrasena.Password;

                Client.client.SendData(JsonConvert.SerializeObject(jugador));

                var result = JsonConvert.DeserializeObject<dynamic>(Client.client.GetData());

                if (result.success == true)
                {
                    MessageBox.Show("Inicio Correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Contraseñas no coinciden", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.psw_contrasena.Password = "";
                this.psw_confirmarContrasena.Password = "";
            }
        }
    }
}
