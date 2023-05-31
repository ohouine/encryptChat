using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    class Client
    {
        IPAddress iPAddress;
        IPEndPoint iPEndPoint;
        TcpClient client;
        public Client()
        {
            iPAddress = IPAddress.Parse("10.5.43.32");
            iPEndPoint = new(iPAddress, 666);
            client = new();
            HandShake();
        }

        public async void HandShake()
        {
            await client.ConnectAsync(iPEndPoint);
        }

        public async void StreamRead()
        {
            await using NetworkStream stream = client.GetStream();
            var buffer = new byte[1_024];
            int received = await stream.ReadAsync(buffer);

            var message = Encoding.UTF8.GetString(buffer, 0, received);
            Debug.WriteLine($"Message received: \"{message}\"");
            // Sample output:
            //     Message received: "📅 8/22/2022 9:07:17 AM 🕛"
        }

        public async void StreamWrite()
        {
            if (client.Connected)
            {
                await using NetworkStream stream = client.GetStream();
                var message = $"📅 {DateTime.Now} 🕛";
                var dateTimeBytes = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(dateTimeBytes);
                Debug.WriteLine($"Sent message: \"{message}\"");
                // Sample output:
                //     Message received: "📅 8/22/2022 9:07:17 AM 🕛"
            }
            else
            {
                HandShake();
            }

        }

    }
}
