using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace ClientConsole.Client
{
    public class SocketClient
    {
        public static Guid ClientId = Guid.NewGuid();

        private static readonly Socket ClientSocket = new Socket
      (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 11000;


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

            Console.Clear();
            Console.WriteLine($"Connected with {ClientId} Id");
        }
        public static void Exit()
        {
            SendString("exit");
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }
        public static void RequestLoop()
        {

            while (true)
            {
                SendRequest();
                ReceiveResponse();
            }
        }

        

        private static string SerializeToString<TData>(TData settings)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, settings);
                stream.Flush();
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        private static void SendString(MessageToSend ms)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(SerializeToString(ms));
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        private static void SendRequest()
        {
            Console.Write("Send a request: ");
            string request = Console.ReadLine();
            MessageToSend messageToSend = new MessageToSend();
            messageToSend.ClientId = ClientId;
            messageToSend.Message = request;
            SendString(messageToSend);
            if (request.ToLower() == "exit")
            {
                Exit();
            }

        }

        private static void SendString(string ms)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(ms);
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