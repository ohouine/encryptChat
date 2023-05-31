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
        IPEndPoint iPEndPoint;
        TcpListener listener;
        TcpClient client;

        public Listener()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Any, 666);
            listener = new TcpListener(iPEndPoint);
            HandShake();
        }
        public async void HandShake()
        {
            if (listener == null)
                return;
            listener.Start();
            try
            {
                client = await listener.AcceptTcpClientAsync();
                StreamRead();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HandShake exception: {ex.Message}");
            }
        }

        public async void StreamWrite(string text)
        {
            await using NetworkStream stream = client.GetStream();
            var message = $"📅 {DateTime.Now} 🕛";
            var dateTimeBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(dateTimeBytes);
            Debug.WriteLine($"Sent message: \"{message}\"");
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
    }
}
