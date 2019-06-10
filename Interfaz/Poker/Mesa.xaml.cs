using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Media;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Poker
{
    public partial class Mesa : Window
    {
        private DispatcherTimer Timer;
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
            Timer.Start();
            this.cantidad = 0;
            this.listener = new ListenerMesa(this);
        }

        public void paint(string datos)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var informacion = JsonConvert.DeserializeObject<dynamic>(datos);

                bote.Text = informacion.table.pot;

                if (informacion.turn == this.jugador.NombreDeUsuario)
                {
                    this.Btn_fold.IsEnabled = true;
                    this.Btn_pass.IsEnabled = true;
                    this.Btn_call.IsEnabled = true;
                    this.Btn_raise.IsEnabled = true;
                    this.Pgb_tiempo.Value = 40;

                    this.jugador.EsSuTurno = true;
                }

                List<Image> fichasDeDealer = new List<Image>();
                fichasDeDealer.Add(Img_ficha_dealer_jugador1);
                fichasDeDealer.Add(Img_ficha_dealer_jugador2);
                fichasDeDealer.Add(Img_ficha_dealer_jugador3);
                fichasDeDealer.Add(Img_ficha_dealer_jugador4);

                for (int i = 0; i < 4; i++)
                {
                    fichasDeDealer[i].Visibility = Visibility.Collapsed;
                }

                List<Ellipse> elipsesDeJugador = new List<Ellipse>();
                elipsesDeJugador.Add(Elp_jugador1);
                elipsesDeJugador.Add(Elp_jugador2);
                elipsesDeJugador.Add(Elp_jugador3);
                elipsesDeJugador.Add(Elp_jugador4);

                for (int i = 0; i < 4; i++)
                {
                    elipsesDeJugador[i].Stroke = (Brush)new BrushConverter().ConvertFrom("Black");
                }

                List<Image> cartasDeMesa = new List<Image>();
                cartasDeMesa.Add(Img_flop1);
                cartasDeMesa.Add(Img_flop2);
                cartasDeMesa.Add(Img_flop3);
                cartasDeMesa.Add(Img_turn);
                cartasDeMesa.Add(Img_river);

                for (int i = 0; i < 5; i++)
                {
                    // Ver si funciona asi
                    cartasDeMesa[i].Source = null;
                }

                for (int i = 0; i < informacion.table.cards.Count; i++)
                {
                    this.Grd_fichas_jugador1.Children.Clear();
                    this.Grd_fichas_jugador2.Children.Clear();
                    this.Grd_fichas_jugador3.Children.Clear();
                    this.Grd_fichas_jugador4.Children.Clear();
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
                    this.pintarFichas(Int32.Parse(apuestasDejugadores[i].Text), i);

                    double minimo = 0;
                    if (Double.Parse(apuestasDejugadores[i].Text) > minimo) 
                    {
                        minimo = Double.Parse(apuestasDejugadores[i].Text);
                        this.Sld_apuesta.Minimum = minimo;
                    }

                    if (informacion.players[i].name == informacion.turn)
                    {
                        elipsesDeJugador[i].Stroke = (Brush)new BrushConverter().ConvertFrom("Red");
                        this.Sld_apuesta.Maximum = Double.Parse(saldosDejugadores[i].Text);

                        if (this.Sld_apuesta.Maximum <= this.Sld_apuesta.Minimum) 
                        {
                            this.Sld_apuesta.Maximum = this.Sld_apuesta.Minimum;
                        }
                    }

                    if (informacion.players[i].name == informacion.dealer)
                    {
                        fichasDeDealer[i].Visibility = Visibility.Visible;
                    }

                    for (int k = 0; k < 2; k++)
                    {
                        cartasDeJugadores[i][k].Source = null;
                    }

                    for (int j = 0; j < informacion.players[i].cards.Count; j++)
                    {
                        if (informacion.players[i].name == this.jugador.NombreDeUsuario)
                        {
                            cartasDeJugadores[i][j].Source = new BitmapImage(new Uri(String.Format("../../Images/Cartas/{0}/{1}.png", informacion.players[i].cards[j].symbol, informacion.players[i].cards[j].number), UriKind.Relative));
                        }
                        // Chequear tambien si el juego ha terminado, con campo 'bool finished' que viene en el JSON
                        else
                        {
                            cartasDeJugadores[i][j].Source = new BitmapImage(new Uri("../../Images/Cartas/carta volteada.jpg", UriKind.Relative));
                        }
                    }
                }

            }), DispatcherPriority.ContextIdle);
        }

        private void pintarFichas(int apuesta, int posicion)
        {
            int cordY = 0;
            cantidad = 0;

            while (apuesta != 0)
            {
                if (apuesta % 1000 == 0)
                {
                    Image ficha = new Image();
                    ficha.Name = "Img_ficha" + posicion + cantidad;

                    if (posicion == 0)
                    {
                        ficha.Margin = new Thickness(301, 115 - cordY, 753, 405 + cordY);
                        this.Grd_fichas_jugador1.Children.Add(ficha);
                    }

                    else if (posicion == 1)
                    {
                        ficha.Margin = new Thickness(753, 115 - cordY, 301, 405 + cordY);
                        this.Grd_fichas_jugador2.Children.Add(ficha);
                    }

                    else if (posicion == 2)
                    {

                        ficha.Margin = new Thickness(753, 410 - cordY, 301, 110 + cordY);
                        this.Grd_fichas_jugador4.Children.Add(ficha);
                    }

                    else if (posicion == 3)
                    {
                        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                        this.Grd_fichas_jugador3.Children.Add(ficha);
                    }

                    cordY += 4;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip1000.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    apuesta -= 1000;
                }

                else if (apuesta % 500 == 0)
                {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + posicion + cantidad;

                    if (posicion == 0)
                    {
                        ficha.Margin = new Thickness(301, 115 - cordY, 753, 405 + cordY);
                        this.Grd_fichas_jugador1.Children.Add(ficha);
                    }

                    else if (posicion == 1)
                    {
                        ficha.Margin = new Thickness(753, 115 - cordY, 301, 405 + cordY);
                        this.Grd_fichas_jugador2.Children.Add(ficha);
                    }

                    else if (posicion == 2)
                    {
                        ficha.Margin = new Thickness(753, 410 - cordY, 301, 110 + cordY);
                        this.Grd_fichas_jugador4.Children.Add(ficha);
                    }

                    else if (posicion == 3)
                    {

                        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                        this.Grd_fichas_jugador3.Children.Add(ficha);
                    }

                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip500.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    apuesta -= 500;
                }

                else if (apuesta % 100 == 0)
                {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + posicion + cantidad;

                    if (posicion == 0)
                    {
                        ficha.Margin = new Thickness(301, 115 - cordY, 753, 405 + cordY);
                        this.Grd_fichas_jugador1.Children.Add(ficha);
                    }

                    else if (posicion == 1)
                    {
                        ficha.Margin = new Thickness(753, 115 - cordY, 301, 405 + cordY);
                        this.Grd_fichas_jugador2.Children.Add(ficha);
                    }

                    else if (posicion == 2)
                    {
                        ficha.Margin = new Thickness(753, 410 - cordY, 301, 110 + cordY);
                        this.Grd_fichas_jugador4.Children.Add(ficha);
                    }

                    else if (posicion == 3)
                    {
                        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                        this.Grd_fichas_jugador3.Children.Add(ficha);
                    }

                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip100.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    apuesta -= 100;
                }

                else if (apuesta % 25 == 0)
                {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + posicion + cantidad;

                    if (posicion == 0)
                    {
                        ficha.Margin = new Thickness(301, 115 - cordY, 753, 405 + cordY);
                        this.Grd_fichas_jugador1.Children.Add(ficha);
                    }

                    else if (posicion == 1)
                    {
                        ficha.Margin = new Thickness(753, 115 - cordY, 301, 405 + cordY);
                        this.Grd_fichas_jugador2.Children.Add(ficha);
                    }

                    else if (posicion == 2)
                    {
                        ficha.Margin = new Thickness(753, 410 - cordY, 301, 110 + cordY);
                        this.Grd_fichas_jugador4.Children.Add(ficha);
                    }

                    else if (posicion == 3)
                    {
                        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                        this.Grd_fichas_jugador3.Children.Add(ficha);
                    }

                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip25.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    apuesta -= 25;
                }

                else if (apuesta % 5 == 0)
                {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + posicion + cantidad;

                    if (posicion == 0)
                    {
                        ficha.Margin = new Thickness(301, 115 - cordY, 753, 405 + cordY);
                        this.Grd_fichas_jugador1.Children.Add(ficha);
                    }

                    else if (posicion == 1)
                    {
                        ficha.Margin = new Thickness(753, 115 - cordY, 301, 405 + cordY);
                        this.Grd_fichas_jugador2.Children.Add(ficha);
                    }

                    else if (posicion == 2)
                    {
                        ficha.Margin = new Thickness(753, 410 - cordY, 301, 110 + cordY);
                        this.Grd_fichas_jugador4.Children.Add(ficha);
                    }

                    else if (posicion == 3)
                    {
                        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                        this.Grd_fichas_jugador3.Children.Add(ficha);
                    }

                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip5.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    apuesta -= 5;
                }

                else
                {
                    Image ficha = new Image();
                    ficha.Name = "Img_fichas" + posicion + cantidad;

                    if (posicion == 0)
                    {
                        ficha.Margin = new Thickness(301, 115 - cordY, 753, 405 + cordY);
                        this.Grd_fichas_jugador1.Children.Add(ficha);
                    }

                    else if (posicion == 1)
                    {
                        ficha.Margin = new Thickness(753, 115 - cordY, 301, 405 + cordY);
                        this.Grd_fichas_jugador2.Children.Add(ficha);
                    }

                    else if (posicion == 2)
                    {
                        ficha.Margin = new Thickness(753, 410 - cordY, 301, 110 + cordY);
                        this.Grd_fichas_jugador4.Children.Add(ficha);
                    }

                    else if (posicion == 3)
                    {
                        ficha.Margin = new Thickness(301, 410 - cordY, 753, 110 + cordY);
                        this.Grd_fichas_jugador3.Children.Add(ficha);
                    }

                    cordY += 4;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("../../Images/Fichas/chip1.png", UriKind.Relative);
                    bitmap.EndInit();

                    ficha.Source = bitmap;
                    ficha.Stretch = Stretch.None;
                    apuesta -= 1;
                }

                cantidad++;
            }
        }

        private void terminarTurno()
        {
            this.jugador.EsSuTurno = false;

            this.Pgb_tiempo.Value = 0;
            this.Btn_fold.IsEnabled = false;
            this.Btn_pass.IsEnabled = false;
            this.Btn_call.IsEnabled = false;
            this.Btn_raise.IsEnabled = false;
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
            this.client.SendData("{\"method\": \"fold\"}");

            new SoundPlayer("../../Sounds/fold.wav").Play();
            this.terminarTurno();
        }

        private void Btn_pass_Click(object sender, RoutedEventArgs e)
        {
            this.client.SendData("{\"method\": \"pass\"}");

            new SoundPlayer("../../Sounds/pass.wav").Play();
            this.terminarTurno();
            
        }

        private void Btn_call_Click(object sender, RoutedEventArgs e)
        {
            this.client.SendData("{\"method\": \"call\"}");

            new SoundPlayer("../../Sounds/raise.wav").Play();
            this.terminarTurno();
        }

        private void Btn_raise_Click(object sender, RoutedEventArgs e)
        {
            this.client.SendData(String.Format("{{\"method\": \"raise\", \"quantity\": {0}}}", (int)Lbl_apuesta.Content));

            new SoundPlayer("../../Sounds/raise.wav").Play();
            this.terminarTurno();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Pgb_tiempo.Value > 0)
            {
                this.Pgb_tiempo.Value -= 1;
            }
        }

        private void Pgb_tiempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Pgb_tiempo.Value == 10)
            {
                new SoundPlayer("../../Sounds/alert.wav").Play();
            }

            else if (this.Pgb_tiempo.Value == 0 && this.jugador.EsSuTurno)
            {
                this.client.SendData("{\"method\": \"fold\"}");
                new SoundPlayer("../../Sounds/fold.wav").Play();
                this.terminarTurno();
            }
        }

        private void Wnd_ventana_ContentRendered(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => this.listener.escucharBroadcasts());
            thread.Start();
        }
    }
}
