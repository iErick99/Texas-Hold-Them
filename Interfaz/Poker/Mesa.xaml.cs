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
using System.Dynamic;
using Newtonsoft.Json;


namespace Poker {
    public partial class Mesa : Window {

        private bool Fuera;
        private SoundPlayer Sonido;
        private DispatcherTimer Timer;
        private bool Ancho;
        private int cantidad;
        private dynamic jugador;
        
        public Mesa() {
            InitializeComponent();
            this.Fuera = false;
            Sonido = new SoundPlayer();
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Start();
            this.Ancho = false;
            this.cantidad = 0;
            this.jugador = new ExpandoObject();
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
            this.Lbl_apuesta.Content = (int)this.Sld_apuesta.Value;
        }

        private void Btn_restar_Click(object sender, RoutedEventArgs e) {
            this.Sld_apuesta.Value -= 1;
        }

        private void Btn_sumar_Click(object sender, RoutedEventArgs e) {
            this.Sld_apuesta.Value += 1;
        }

        private void Btn_fold_Click(object sender, RoutedEventArgs e) {
            if (!this.Fuera) {
                this.Img_carta1_jugador3.Visibility = Visibility.Hidden;
                this.Img_carta2_jugador3.Visibility = Visibility.Hidden;
                Sonido.SoundLocation = "../../Sounds/fold.wav";
                Sonido.Play();
                this.Fuera = true;
            }
        }

        private void Elp_jugador3_MouseEnter(object sender, MouseEventArgs e) {
            
            if (this.Fuera) {
                this.Img_carta1_jugador3.Opacity = 0.60;
                this.Img_carta2_jugador3.Opacity = 0.60;
                this.Img_carta1_jugador3.Visibility = Visibility.Visible;
                this.Img_carta2_jugador3.Visibility = Visibility.Visible;
            }
        }

        private void Elp_jugador3_MouseLeave(object sender, MouseEventArgs e) {
            if (this.Fuera) {
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
            if (this.Pgb_tiempo.Value > 0 && !Fuera) {
                this.Pgb_tiempo.Value -= 1;

                if (!Ancho) {
                    this.Elp_jugador3.StrokeThickness = 5;
                    Ancho = true;
                }

                else {
                    this.Elp_jugador3.StrokeThickness = 1;
                    Ancho = false;
                }
            }

            else {
                this.Elp_jugador3.StrokeThickness = 1;
                Ancho = false;
                this.Pgb_tiempo.Value = 15;
            }
        }

        private void Btn_raise_Click(object sender, RoutedEventArgs e) {
            Sonido.SoundLocation = "../../Sounds/raise.wav";
            Sonido.Play();

            this.Tbk_apuesta_jugador3.Text = this.Lbl_apuesta.Content + "";
            int apuesta = Int32.Parse(Tbk_apuesta_jugador3.Text);

            this.Tbk_saldo_jugador3.Text = Int32.Parse(this.Tbk_saldo_jugador3.Text) - (int)this.Lbl_apuesta.Content + "";
            Fuera = true;

            
            this.jugador.method = "raise";
            this.jugador.raise = (int)Lbl_apuesta.Content + "";
            this.Sld_apuesta.Value = 0;

            var result = JsonConvert.DeserializeObject<dynamic>(Poker.Client.cliente.SendRequest(JsonConvert.SerializeObject(jugador)));
 
            int cordX = 28, cordY = 0;

            if (cantidad != 0) {
                this.Grd_fichas.Children.Clear();
                cantidad = 0;
            }

            while (apuesta != 0) {
                if (apuesta % 1000 == 0) {
                    Image ficha = new Image();
                    ficha.Name = "Img_ficha" + cantidad;
                    ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip1000.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    this.Grd_fichas.Children.Add(ficha);
                    apuesta -= 1000;
                }

                else if (apuesta % 500 == 0) {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + cantidad;
                    ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip500.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    this.Grd_fichas.Children.Add(ficha);
                    apuesta -= 500;
                }

                else if (apuesta % 100 == 0) {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + cantidad;
                    ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip100.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    this.Grd_fichas.Children.Add(ficha);
                    apuesta -= 100;
                }

                else if (apuesta % 25 == 0) {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + cantidad;
                    ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip25.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    this.Grd_fichas.Children.Add(ficha);
                    apuesta -= 25;
                }

                else if (apuesta % 5 == 0) {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + cantidad;
                    ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip5.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    this.Grd_fichas.Children.Add(ficha);
                    apuesta -= 5;
                }

                else {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + cantidad;
                    ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip1.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    this.Grd_fichas.Children.Add(ficha);
                    apuesta -= 1;
                }

                cantidad++;
            }
        }

        private void Btn_pass_Click(object sender, RoutedEventArgs e) {
            Sonido.SoundLocation = "../../Sounds/pass.wav";
            Sonido.Play();
        }

        private void Pgb_tiempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (this.Pgb_tiempo.Value == 5) {
                Sonido.SoundLocation = "../../Sounds/alert.wav";
                Sonido.Play();
            }

            else if (this.Pgb_tiempo.Value == 0) {
                this.Btn_fold_Click(sender, e);
            }
        }
    }
}
