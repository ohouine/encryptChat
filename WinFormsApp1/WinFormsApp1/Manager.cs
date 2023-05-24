using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WinFormsApp1
{
    class Manager
    {

        public async void TestSend()
        {

            IPAddress ipAddress = IPAddress.Parse("10.tamere...");
            IPEndPoint ipEndPoint = new(ipAddress, 666);

            ipEndPoint = new IPEndPoint(ipAddress, 13);

            using TcpClient client = new();
            await client.ConnectAsync(ipEndPoint);
            await using NetworkStream stream = client.GetStream();

            var buffer = new byte[1_024];
            int received = await stream.ReadAsync(buffer);

            var message = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine($"Message received: \"{message}\"");
            // Sample output:
            //     Message received: "📅 8/22/2022 9:07:17 AM 🕛"
        }

        public async void Listener()
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 666);
            TcpListener listener = new(ipEndPoint);

            try
            {
                listener.Start();

                using TcpClient handler = await listener.AcceptTcpClientAsync();
                await using NetworkStream stream = handler.GetStream();

                var message = $"📅 {DateTime.Now} 🕛";
                var dateTimeBytes = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(dateTimeBytes);

                Console.WriteLine($"Sent message: \"{message}\"");
                // Sample output:
                //     Sent message: "📅 8/22/2022 9:07:17 AM 🕛"
            }
            finally
            {
                listener.Stop();
            }
        }

    }
}
