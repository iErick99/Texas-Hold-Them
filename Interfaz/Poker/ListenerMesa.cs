namespace Poker
{
    class ListenerMesa
    {
        Mesa mesa;

        public ListenerMesa(Mesa mesa)
        {
            this.mesa = mesa;
        }
        public void escuchar()
        {
            while (true)
            {
                this.mesa.paint(Client.client.GetData());
            }
        }
    }
}
