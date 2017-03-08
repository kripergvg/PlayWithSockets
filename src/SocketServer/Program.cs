using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ipHost = Dns.GetHostEntryAsync("localhost").Result;
            var ipAddr = ipHost.AddressList.First();

            var listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            var ipEndPoint = new IPEndPoint(ipAddr, 11000);
            listener.Bind(ipEndPoint);
            listener.Listen(10);

            var handler = listener.Accept();
             
            var bytes = new byte[10];

            while (true)
            {
                var receivedBytes = handler.Receive(bytes);

                var data = Encoding.UTF8.GetString(bytes, 0, receivedBytes);
                Console.WriteLine($"Received text \"{data}\"");

                var message = Encoding.UTF8.GetBytes($"Response message for message with size {data.Length}");
                handler.Send(message);

                if (data.Contains("Конец"))
                {
                    Console.WriteLine("Пришла команда окончания");
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Dispose();
                }
            }
        }
    }
}
