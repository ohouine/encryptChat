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
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace WinFormsApp1
{
    class Listener
    {
        IPEndPoint iPEndPoint;
        TcpListener listener;
        TcpClient client;
        byte[] _nonce;
        byte[] _key;
        int handShakeStep = 0;

        public Listener()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Any, 666);
            listener = new TcpListener(iPEndPoint);
            _key = SecretBox.GenerateKey();
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
                var textBytes = Encoding.UTF8.GetBytes("hello");
                MessageBox.Show($"Sent message: \"hello\"");
                HandShakeStep(textBytes);
                handShakeStep++;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HandShake exception: {ex.Message}");
            }
        }

      /*  public async void StreamWrite(string text)
        {
            await using NetworkStream stream = client.GetStream();

            byte[] key = SecretBox.GenerateKey();
            var message = $"📅 {DateTime.Now} 🕛";
            var dateTimeBytes = Encoding.UTF8.GetBytes(message);
            byte[] toSend = new byte[nonce.Length + key.Length + message.Length];
            toSend.Concat(nonce.Concat(key.Concat(dateTimeBytes)));
            await stream.WriteAsync(toSend);
            Debug.WriteLine($"Sent message: \"{message}\"");
        }*/
        public async void StreamRead()
        {
            while (true)
            {
                try
                {
                    await using NetworkStream stream = client.GetStream();
                    var buffer = new byte[1_024];
                    int received = await stream.ReadAsync(buffer);
                    HandShakeStep(buffer);
                    var message = Encoding.UTF8.GetString(buffer, 0, received);
                    MessageBox.Show($"Message received: \"{message}\"");
                }
                catch (Exception ex)
                {

                }   
            }
            
        }
        public async void HandShakeStep(byte[] buffer)
        {
            switch (handShakeStep)
            {
                case 0:
                    {
                        await using NetworkStream stream = client.GetStream();
                        await stream.WriteAsync(buffer);
                        handShakeStep++;
                        break;
                    }
                    case 1:
                    {
                        if (buffer.Length == 24)
                        {
                            _nonce = buffer;
                            await using NetworkStream stream = client.GetStream();
                            await stream.WriteAsync(_key);
                            MessageBox.Show($"Sent message: \"{_key}\"");
                            handShakeStep++;
                        }
                        break;
                    }
            }
        }
    }
}
