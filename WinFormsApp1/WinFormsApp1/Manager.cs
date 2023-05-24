using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WinFormsApp1
{
    class Manager
    {

        public async void TestSend(string text)
        {

            Debug.WriteLine($"Start sending:");

            IPAddress ipAddress = IPAddress.Parse("10.5.43.37");
            IPEndPoint ipEndPoint = new(ipAddress, 666);

            ipEndPoint = new IPEndPoint(ipAddress, 666);

            using TcpClient client = new();
            await client.ConnectAsync(ipEndPoint);
            await using NetworkStream stream = client.GetStream();

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            int received = await stream.ReadAsync(buffer);

            var message = Encoding.UTF8.GetString(buffer, 0, received);
            Debug.WriteLine($"Message received: \"{message}\"");
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
                await using (NetworkStream stream = handler.GetStream()) { 
                
                }
            }
            finally
            {
                listener.Stop();
            }
        }

    }
}
