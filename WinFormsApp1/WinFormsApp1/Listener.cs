using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Sodium;
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
            byte[] nonce = SecretBox.GenerateNonce();
            byte[] key = SecretBox.GenerateKey();
            var message = $"📅 {DateTime.Now} 🕛";
            var dateTimeBytes = Encoding.UTF8.GetBytes(message);
            byte[] toSend = new byte[nonce.Length + key.Length + message.Length];
            toSend.Concat(nonce.Concat(key.Concat(dateTimeBytes)));
            await stream.WriteAsync(toSend);
            Debug.WriteLine($"Sent message: \"{message}\"");
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
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
