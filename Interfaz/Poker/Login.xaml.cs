using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Poker
{

    public partial class Login : Window
    {

        private Cuenta cuenta;
        private Mesa mesa;
        private dynamic jugador;

        public Login()
        {
            InitializeComponent();
            this.cuenta = null;
            this.mesa = null;
            this.jugador = new ExpandoObject();

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
            if (this.cuenta == null)
            {
                this.cuenta = new Cuenta();
            }

            this.Hide();
            this.cuenta.Show();
        }

        private void Btn_iniciar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Client.client.Connect("13.90.205.129", 100);

                this.jugador.method = "login";
                this.jugador.user = txt_usuario.Text;
                this.jugador.password = "123";

                Client.client.SendData(JsonConvert.SerializeObject(jugador));

                var result = JsonConvert.DeserializeObject<dynamic>(Client.client.GetData());

                if (result.success == true)
                {
                    this.mesa = new Mesa(txt_usuario.Text);
                    MessageBox.Show("Inicio Correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Hide();
                    this.mesa.Show();
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    Client.client.Connect("13.90.205.129", 100);

                    this.jugador.method = "login";
                    this.jugador.user = txt_usuario.Text;
                    this.jugador.password = psw_contrasena.Password;

                    Client.client.SendData(JsonConvert.SerializeObject(jugador));

                    var result = JsonConvert.DeserializeObject<dynamic>(Client.client.GetData());

                    if (result.success == true)
                    {
                        this.mesa = new Mesa(txt_usuario.Text);
                        MessageBox.Show("Inicio Correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Hide();
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
}
