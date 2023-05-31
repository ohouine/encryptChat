using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;

namespace WinFormsApp1
{
    class Manager
    {
        IPAddress iPAddress;
        IPEndPoint iPEndPoint;
        TcpClient client;
        TcpListener listener;


        public Manager()
        {
            iPAddress = IPAddress.Parse("10.5.43.32");
            iPEndPoint = new(iPAddress, 666);
            client = new ();
            CreateTcpClient();
            listener = CreateListener();
        }

        private async void CreateTcpClient()
        {
            await client.ConnectAsync(iPEndPoint);
        }
        public async void StreamWrite(string text)
        {
            bool isOpen = false;
            listener.Start();
            while (!isOpen)
            {
                try
                {
                    client = await listener.AcceptTcpClientAsync();
                    await using NetworkStream stream = client.GetStream();
                    var message = $"📅 {DateTime.Now} 🕛";
                    var dateTimeBytes = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(dateTimeBytes);
                    Debug.WriteLine($"Sent message: \"{message}\"");
                    // Sample output:
                    //     Sent message: "📅 8/22/2022 9:07:17 AM 🕛"

                }
                finally
                {
                    isOpen = true;
                }
            }
            listener.Stop();

        }
        public async void StreamRead()
        {
            bool isOpen = false;
            listener.Start();
            while (!isOpen)
            {
                try
                {
                    client = await listener.AcceptTcpClientAsync();
                    await using NetworkStream stream = client.GetStream();
                    var buffer = new byte[1_024];
                    int received = await stream.ReadAsync(buffer);
                    var message = Encoding.UTF8.GetString(buffer, 0, received);
                    // Sample output:
                    //     Sent message: "📅 8/22/2022 9:07:17 AM 🕛"

                }
                finally
                {
                    isOpen = true;
                }
            }
            listener.Stop();
        }

        private TcpListener CreateListener()
        {
            TcpListener listener = new(iPEndPoint);
            return listener;
        }
    }
}

