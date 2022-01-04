using System;
using ClientConsole.Client;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //SocketClient.StartClient();
            Console.Title = "Client";
            SocketClient.ConnectToServer();
            SocketClient.RequestLoop();
            SocketClient.Exit();
        }
    }
}
