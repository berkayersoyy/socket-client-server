using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerConsole.Constants;

namespace ClientConsole.Client
{
    public class SocketClient
    {
        private static readonly Socket ClientSocket = new Socket
      (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 100;


        public static void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            }

            var ipAddress = IPAddress.Parse(((IPEndPoint)ClientSocket.RemoteEndPoint).Address.ToString());
            var port = IPAddress.Parse(((IPEndPoint)ClientSocket.RemoteEndPoint).Port.ToString());
            Console.Clear();
            ClientCount.ClientCounter++;
            Console.WriteLine($"Connected as a {ClientCount.ClientCounter} client with "+ ipAddress + ":" + port);
        }
        public static void Exit()
        {
            SendString("exit");
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
            ClientCount.ClientCounter--;
        }
        public static void RequestLoop()
        {

            while (true)
            {
                SendRequest();
                ReceiveResponse();
            }
        }

       

        private static void SendRequest()
        {
            Console.Write("Send a request: ");
            string request = Console.ReadLine();
            SendString(request);
            if (request.ToLower() == "exit")
            {
                Exit();
            }

        }

   
        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Console.WriteLine(text);
        }
    }
}