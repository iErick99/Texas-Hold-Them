namespace Poker
{
    class ListenerMesa
    {
        Mesa mesa;

        public ListenerMesa(Mesa mesa)
        {
            this.mesa = mesa;
        }
        public void escucharBroadcasts()
        {
            while (true)
            {
                this.mesa.paint(this.mesa.client.GetData());
            }
        }
    }
}
