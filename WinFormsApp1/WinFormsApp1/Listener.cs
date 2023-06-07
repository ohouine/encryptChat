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
        const int NonceLen = 24;
        const int HandShakeStepCount = 3;
        IPEndPoint iPEndPoint;
        TcpListener listener;
        TcpClient client;
        byte[] _nonce;
        byte[] _key;
        int handShakeStep;

        public Listener()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Any, 666);
            listener = new TcpListener(iPEndPoint);
            _key = SecretBox.GenerateKey();
            handShakeStep = 0;
        }
        public async Task<bool> HandShake()
        {
            listener.Start();
            while (handShakeStep < HandShakeStepCount)
            {
                Debug.WriteLine("HandShake - handShakeStep=" + handShakeStep);
                switch (handShakeStep)
                {
                    case 0:
                        {
                            bool bRetValue = await AcceptClientAsync();
                            if (!bRetValue)
                            {
                                Debug.WriteLine("HandShake - fail to connect");
                                return false;
                            }
                            // Next step
                            handShakeStep++;
                            Debug.WriteLine("HandShake - connected");
                            break;
                        }
                    case 1:
                        {
                            bool bRetValue = await InternalStreamWriteAsync(Encoding.UTF8.GetBytes("Hello"));
                            if (!bRetValue)
                            {
                                Debug.WriteLine("HandShake - fail to send Hello");
                                return false;
                            }
                            // Next step
                            handShakeStep++;
                            Debug.WriteLine("HandShake - sended Hello");
                            break;
                        }
                    case 2:
                        {
                            byte[] buffer = await InternalStreamReadAsync();
                            if (buffer.Length != NonceLen)
                            {
                                Debug.WriteLine("HandShake - fail to receive nonce");
                                return false;
                            }
                            // Next step
                            handShakeStep++;
                            Debug.WriteLine("HandShake - received nonce");
                            break;
                        }
                }
            }
            listener.Stop();
            return true;
        }
        private async Task<bool> AcceptClientAsync()
        {
            try
            {
                client = await listener.AcceptTcpClientAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AcceptClientAsync - Exception - {ex.Message}");
                return false;
            }
            return true;
        }
        private async Task<byte[]> InternalStreamReadAsync()
        {
            var buffer = new byte[1_024];
            try
            {
                await using NetworkStream stream = client.GetStream();
                int received = await stream.ReadAsync(buffer);
                if (received > 0)
                {
                    Debug.WriteLine("InternalStreamReadAsync - buffer received: " + BitConverter.ToString(buffer));
                }
                else
                {
                    Debug.WriteLine("InternalStreamReadAsync - buffer received empty ");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"InternalStreamReadAsync  -  Exception - {ex.Message}");
            }
            return buffer;
        }

        private async Task<bool> InternalStreamWriteAsync(byte[] buffer)
        {
            try
            {
                await using NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"InternalStreamWriteAsync  -  Exception - {ex.Message}");
                return false;
            }
            // Done
            return true;
        }
    }
}