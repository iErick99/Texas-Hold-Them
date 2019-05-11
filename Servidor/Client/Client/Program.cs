using System;
using System.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            // Aca puse la dir. IP loopback por mientras para que no tengan que estarla cambiando al correrlo
            client.Connect((IPAddress.Loopback).ToString(), 100);

            // Recordar que las requests al server DEBEN ir en formato JSON. Si se manda otra cosa va a a petar porque no va a poder de-serializar esa cadena
            Console.WriteLine(String.Format("Server's response: {0}", client.SendRequest("{\"method\": \"login\"}")));
            Console.ReadLine();
        }
    }
}
