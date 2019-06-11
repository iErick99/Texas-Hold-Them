using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// <summary>
    /// Lógica de interacción para Servidor.xaml
    /// </summary>
    public partial class Servidor : Window {

        Client client;
        Login login;
        public Servidor() {
            InitializeComponent();
            client = new Client();
            login = new Login(client);
        }

        private void Brd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }


        private void Btn_cerrar_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Btn_iniciar_Click(object sender, RoutedEventArgs e) {
            try {
                new SoundPlayer("../../Sounds/connect.wav").Play();

                client.Connect(this.txt_ip.Text, Int32.Parse(this.txt_puerto.Text));
                MessageBox.Show("Conectado correctamente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                login.Show();
                this.Hide();
            }

            catch (Exception exc) {
                MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
