using System;
using ServerConsole.Server;

namespace ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //SocketListener.StartServer();
            Console.Title = "Server";
            SocketListener.SetupServer();
            Console.ReadLine(); // When we press enter close everything
            SocketListener.CloseAllSockets();
        }
    }
}
