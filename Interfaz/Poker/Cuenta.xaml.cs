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
        private Client client;

        public Cuenta(Client client)
        {
            InitializeComponent();
            this.jugador = new ExpandoObject();
            this.client = client;
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
            if (this.psw_confirmarContrasena.Password.Equals(this.psw_contrasena.Password)) {
                client.Connect(this.txt_ip.Text, Int32.Parse(this.txt_puerto.Text));

                jugador.method = "create";
                jugador.user = this.txt_usuario.Text;
                jugador.password = this.psw_contrasena.Password;

                client.SendData(JsonConvert.SerializeObject(jugador));

                var result = JsonConvert.DeserializeObject<dynamic>(client.GetData());

                if (result.success == true) {
                    MessageBox.Show("Creada correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Hide();
                }

                else {
                    MessageBox.Show("No se pudo crear la cuenta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else {
                MessageBox.Show("Contraseñas no coinciden", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.psw_contrasena.Password = "";
                this.psw_confirmarContrasena.Password = "";
            }
        }

        private void Btn_volver_Click(object sender, RoutedEventArgs e) 
        {
            this.Hide();
        }
    }
}
