namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("172.17.30.145", 100);

            server.Start();
            server.Run();
        }
    }
}
