using Newtonsoft.Json;
using System;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace Poker
{

    public partial class Login : Window
    {
        Client client;
        Cuenta cuenta;

        public Login(Client client)
        {
            InitializeComponent();
            this.client = client;
            cuenta = new Cuenta(client);
        }

        private void Brd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void Btn_cerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Hl_registrate_Click(object sender, RoutedEventArgs e)
        {
            this.cuenta.Show();
        }

        private void Btn_iniciar_Click(object sender, RoutedEventArgs e)
        {
            try 
            {

                this.client.SendData(String.Format("{{\"method\": \"login\", \"user\": \"{0}\", \"password\": \"{1}\"}}", txt_usuario.Text, psw_contrasena.Password));

                var result = JsonConvert.DeserializeObject<dynamic>(this.client.GetData());

                if (result.success == true)
                {
                    Mesa mesa = new Mesa(txt_usuario.Text, this.client);
                    MessageBox.Show("Inicio Correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.Hide();
                    mesa.Show();
                }

                else
                {
                    MessageBox.Show("Credenciales incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}