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
    class Listener
    {
        IPAddress iPAddress;
        IPEndPoint iPEndPoint;
        TcpListener listener;
        TcpClient handler;

        public Listener()
        {
            iPAddress = IPAddress.Parse("10.5.43.32");
            iPEndPoint = new(iPAddress, 666);
            listener = new(iPEndPoint);
            HandShake();
        }
        public async void HandShake()
        {
            listener.Start();
            if (listener == null)
                return;
            handler = await listener.AcceptTcpClientAsync();
        }

        public async void StreamWrite(string text)
        {
            await using NetworkStream stream = handler.GetStream();
            var message = $"📅 {DateTime.Now} 🕛";
            var dateTimeBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(dateTimeBytes);
            Debug.WriteLine($"Sent message: \"{message}\"");
        }
        public async void StreamRead()
        {
            using TcpClient handler = await listener.AcceptTcpClientAsync();
            await using NetworkStream stream = handler.GetStream();
            var buffer = new byte[1_024];
            int received = await stream.ReadAsync(buffer);
            var message = Encoding.UTF8.GetString(buffer, 0, received);
            Debug.WriteLine($"Message received: \"{message}\"");
            // Sample output:
            //     Message received: "📅 8/22/2022 9:07:17 AM 🕛"
        }
    }
}
