using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Media;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;

namespace Poker
{
    public partial class Mesa : Window
    {
        private DispatcherTimer Timer;
        private bool Ancho;
        private bool esMiTurno;
        private int cantidad;
        private Jugador jugador;
        private ListenerMesa listener;
        public Client client { get; set; }

        public Mesa(String nombreDeUsuario, Client client)
        {
            InitializeComponent();
            this.client = client;
            this.jugador = new Jugador();
            this.jugador.NombreDeUsuario = nombreDeUsuario;

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = TimeSpan.FromSeconds(1);
            this.Ancho = false;
            this.cantidad = 0;
            this.listener = new ListenerMesa(this);
        }

        public void paint(string datos)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var informacion = JsonConvert.DeserializeObject<dynamic>(datos);

                if (informacion.turn == this.jugador.NombreDeUsuario)
                {
                    this.Btn_fold.IsEnabled = true;
                    this.Btn_pass.IsEnabled = true;
                    this.Btn_call.IsEnabled = true;
                    this.Btn_raise.IsEnabled = true;
                    this.Pgb_tiempo.Value = 40;
                    Timer.Start();

                    this.esMiTurno = true;
                }
                else
                {
                    this.Btn_fold.IsEnabled = false;
                    this.Btn_pass.IsEnabled = false;
                    this.Btn_call.IsEnabled = false;
                    this.Btn_raise.IsEnabled = false;
                }

                bote.Text = informacion.table.pot;

                List<Image> cartasDeMesa = new List<Image>();
                cartasDeMesa.Add(Img_flop1);
                cartasDeMesa.Add(Img_flop2);
                cartasDeMesa.Add(Img_flop3);
                cartasDeMesa.Add(Img_turn);
                cartasDeMesa.Add(Img_river);

                for (int i = 0; i < informacion.table.cards.Count; i++)
                {
                    cartasDeMesa[i].Source = new BitmapImage(new Uri(String.Format("../../Images/Cartas/{0}/{1}.png", informacion.table.cards[i].symbol, informacion.table.cards[i].number), UriKind.Relative));
                }

                List<TextBlock> nombresDejugadores = new List<TextBlock>();
                nombresDejugadores.Add(Tbk_jugador1);
                nombresDejugadores.Add(Tbk_jugador2);
                nombresDejugadores.Add(Tbk_jugador3);
                nombresDejugadores.Add(Tbk_jugador4);

                List<TextBlock> saldosDejugadores = new List<TextBlock>();
                saldosDejugadores.Add(Tbk_saldo_jugador1);
                saldosDejugadores.Add(Tbk_saldo_jugador2);
                saldosDejugadores.Add(Tbk_saldo_jugador3);
                saldosDejugadores.Add(Tbk_saldo_jugador4);

                List<TextBlock> apuestasDejugadores = new List<TextBlock>();
                apuestasDejugadores.Add(Tbk_apuesta_jugador1);
                apuestasDejugadores.Add(Tbk_apuesta_jugador2);
                apuestasDejugadores.Add(Tbk_apuesta_jugador3);
                apuestasDejugadores.Add(Tbk_apuesta_jugador4);

                List<List<Image>> cartasDeJugadores = new List<List<Image>>();
                List<Image> cartasDeJugador1 = new List<Image>();
                cartasDeJugador1.Add(Img_carta1_jugador1);
                cartasDeJugador1.Add(Img_carta2_jugador1);
                cartasDeJugadores.Add(cartasDeJugador1);

                List<Image> cartasDeJugador2 = new List<Image>();
                cartasDeJugador2.Add(Img_carta1_jugador2);
                cartasDeJugador2.Add(Img_carta2_jugador2);
                cartasDeJugadores.Add(cartasDeJugador2);

                List<Image> cartasDeJugador3 = new List<Image>();
                cartasDeJugador3.Add(Img_carta1_jugador3);
                cartasDeJugador3.Add(Img_carta2_jugador3);
                cartasDeJugadores.Add(cartasDeJugador3);

                List<Image> cartasDeJugador4 = new List<Image>();
                cartasDeJugador4.Add(Img_carta1_jugador4);
                cartasDeJugador4.Add(Img_carta2_jugador4);
                cartasDeJugadores.Add(cartasDeJugador4);

                for (int i = 0; i < informacion.players.Count; i++)
                {
                    nombresDejugadores[i].Text = informacion.players[i].name;
                    saldosDejugadores[i].Text = informacion.players[i].balance;
                    apuestasDejugadores[i].Text = informacion.players[i].bet;

                    for (int j = 0; j < informacion.players[i].cards.Count; j++)
                    {
                        if (informacion.players[i].name == this.jugador.NombreDeUsuario)
                        {
                            cartasDeJugadores[i][j].Source = new BitmapImage(new Uri(String.Format("../../Images/Cartas/{0}/{1}.png", informacion.players[i].cards[j].symbol, informacion.players[i].cards[j].number), UriKind.Relative));
                        }
                        else
                        {
                            cartasDeJugadores[i][j].Source = new BitmapImage(new Uri("../../Images/Cartas/carta volteada.jpg", UriKind.Relative));
                        }
                    }
                }

                // Pintar las fichillas de cada bichillo ah?
                //int cordY = 0;

                //if (cantidad != 0)
                //{
                //    this.Grd_fichas.Children.Clear();
                //    cantidad = 0;
                //}

                //while (apuesta != 0)
                //{
                //    if (apuesta % 1000 == 0)
                //    {
                //        Image ficha = new Image();
                //        ficha.Name = "Img_ficha" + cantidad;
                //        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                //        cordY += 4;

                //        BitmapImage bitmap = new BitmapImage();
                //        bitmap.BeginInit();
                //        bitmap.UriSource = new Uri("../../Images/Fichas/chip1000.png", UriKind.Relative);
                //        bitmap.EndInit();

                //        ficha.Source = bitmap;
                //        ficha.Stretch = Stretch.None;
                //        this.Grd_fichas.Children.Add(ficha);
                //        apuesta -= 1000;
                //    }

                //    else if (apuesta % 500 == 0)
                //    {
                //        Image ficha = new Image();
                //        ficha.Name = "Img_fichas" + cantidad;
                //        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                //        cordY += 4;

                //        BitmapImage bitmap = new BitmapImage();
                //        bitmap.BeginInit();
                //        bitmap.UriSource = new Uri("../../Images/Fichas/chip500.png", UriKind.Relative);
                //        bitmap.EndInit();

                //        ficha.Source = bitmap;
                //        ficha.Stretch = Stretch.None;
                //        this.Grd_fichas.Children.Add(ficha);
                //        apuesta -= 500;
                //    }

                //    else if (apuesta % 100 == 0)
                //    {
                //        Image ficha = new Image();
                //        ficha.Name = "Img_fichas" + cantidad;
                //        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                //        cordY += 4;

                //        BitmapImage bitmap = new BitmapImage();
                //        bitmap.BeginInit();
                //        bitmap.UriSource = new Uri("../../Images/Fichas/chip100.png", UriKind.Relative);
                //        bitmap.EndInit();

                //        ficha.Source = bitmap;
                //        ficha.Stretch = Stretch.None;
                //        this.Grd_fichas.Children.Add(ficha);
                //        apuesta -= 100;
                //    }

                //    else if (apuesta % 25 == 0)
                //    {
                //        Image ficha = new Image();
                //        ficha.Name = "Img_fichas" + cantidad;
                //        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                //        cordY += 4;

                //        BitmapImage bitmap = new BitmapImage();
                //        bitmap.BeginInit();
                //        bitmap.UriSource = new Uri("../../Images/Fichas/chip25.png", UriKind.Relative);
                //        bitmap.EndInit();

                //        ficha.Source = bitmap;
                //        ficha.Stretch = Stretch.None;
                //        this.Grd_fichas.Children.Add(ficha);
                //        apuesta -= 25;
                //    }

                //    else if (apuesta % 5 == 0)
                //    {
                //        Image ficha = new Image();
                //        ficha.Name = "Img_fichas" + cantidad;
                //        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                //        cordY += 4;

                //        BitmapImage bitmap = new BitmapImage();
                //        bitmap.BeginInit();
                //        bitmap.UriSource = new Uri("../../Images/Fichas/chip5.png", UriKind.Relative);
                //        bitmap.EndInit();

                //        ficha.Source = bitmap;
                //        ficha.Stretch = Stretch.None;
                //        this.Grd_fichas.Children.Add(ficha);
                //        apuesta -= 5;
                //    }

                //    else
                //    {
                //        Image ficha = new Image();
                //        ficha.Name = "Img_fichas" + cantidad;
                //        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                //        cordY += 4;

                //        BitmapImage bitmap = new BitmapImage();
                //        bitmap.BeginInit();
                //        bitmap.UriSource = new Uri("../../Images/Fichas/chip1.png", UriKind.Relative);
                //        bitmap.EndInit();

                //        ficha.Source = bitmap;
                //        ficha.Stretch = Stretch.None;
                //        this.Grd_fichas.Children.Add(ficha);
                //        apuesta -= 1;
                //    }

                //    cantidad++;
                //}

            }), DispatcherPriority.ContextIdle);
        }

        private void Btn_cerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Brd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.wnd_ventana.WindowState = WindowState.Minimized;
        }

        private void Sld_apuesta_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Lbl_apuesta.Content = (int)this.Sld_apuesta.Value;
        }

        private void Btn_restar_Click(object sender, RoutedEventArgs e)
        {
            this.Sld_apuesta.Value -= 1;
        }

        private void Btn_sumar_Click(object sender, RoutedEventArgs e)
        {
            this.Sld_apuesta.Value += 1;
        }

        private void Btn_fold_Click(object sender, RoutedEventArgs e)
        {
            this.client.SendData("{{\"method\": \"fold\"}}");

            new SoundPlayer("../../Sounds/fold.wav").Play();
            this.esMiTurno = false;
        }

        private void Btn_pass_Click(object sender, RoutedEventArgs e)
        {
            if (Tbk_apuesta_jugador1.Text.Equals("0") || Tbk_apuesta_jugador2.Text.Equals("0") || Tbk_apuesta_jugador3.Text.Equals("0") || Tbk_apuesta_jugador4.Text.Equals("0")) 
            {
                this.client.SendData("{{\"method\": \"pass\"}}");

                new SoundPlayer("../../Sounds/pass.wav").Play();
                this.esMiTurno = false;
            }
        }

        private void Btn_call_Click(object sender, RoutedEventArgs e)
        {
            this.client.SendData("{{\"method\": \"call\"}}");

            new SoundPlayer("../../Sounds/raise.wav").Play();
            this.esMiTurno = false;
        }

        private void Btn_raise_Click(object sender, RoutedEventArgs e)
        {
            this.client.SendData(String.Format("{{\"method\": \"raise\", \"quantity\": {0}}}", (int)Lbl_apuesta.Content));

            new SoundPlayer("../../Sounds/raise.wav").Play();
            this.esMiTurno = false;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Pgb_tiempo.Value > 0)
            {
                this.Pgb_tiempo.Value -= 1;

                if (!Ancho)
                {
                    this.Elp_jugador3.StrokeThickness = 5;
                    Ancho = true;
                }

                else
                {
                    this.Elp_jugador3.StrokeThickness = 1;
                    Ancho = false;
                }
            }

            else
            {
                this.Elp_jugador3.StrokeThickness = 1;
                Ancho = false;
                this.Pgb_tiempo.Value = 15;
            }
        }

        private void Pgb_tiempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Pgb_tiempo.Value == 5)
            {
                new SoundPlayer("../../Sounds/alert.wav").Play();
            }

            else if (this.Pgb_tiempo.Value == 0 && esMiTurno)
            {
                this.client.SendData("{{\"method\": \"fold\"}}");
                new SoundPlayer("../../Sounds/fold.wav").Play();
            }
        }

        private void Wnd_ventana_ContentRendered(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => this.listener.escucharBroadcasts());
            thread.Start();
        }
    }
}
