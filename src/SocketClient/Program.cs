using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bytes = new byte[10];

            var ipHost = Dns.GetHostEntryAsync("localhost").Result;
            var ipAddr = ipHost.AddressList.First();
            var ipEndpoint = new IPEndPoint(ipAddr, 11000);

            var sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(ipEndpoint);

            while (true)
            {
                Console.WriteLine("Enter Message"); 
                var message = Console.ReadLine();
                var messageEncoded = Encoding.UTF8.GetBytes(message);

                int bytesSent = sender.Send(messageEncoded);

                int bytesRecieved = sender.Receive(bytes);

                Console.WriteLine($"Response from server {Encoding.UTF8.GetString(bytes, 0, bytesRecieved)}");

                if (message.Contains("Конец"))
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Dispose();
                }
            }



        }
    }
}
 