using Sodium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WinFormsApp1
{
    class Client
    {
        const int HandShakeStepCount = 3;

        IPAddress iPAddress;
        IPEndPoint iPEndPoint;
        TcpClient client;
        byte[] _nonce;
        byte[] _key;
        int handShakeStep;



        public Client(string ip)
        {
            iPAddress = IPAddress.Parse(ip);
            iPEndPoint = new(iPAddress, 666);
            _nonce = SecretBox.GenerateNonce();
            client = new TcpClient();
            handShakeStep = 0;
        }

        public async Task<bool> HandShake()
        {
            while (handShakeStep < HandShakeStepCount)
            {
                Debug.WriteLine("HandShake - handShakeStep=" + handShakeStep);
                switch (handShakeStep)
                {
                    case 0:
                        {
                            bool bRetValue = await ConnectClientAsync();
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
                            byte[] buffer = await InternalStreamReadAsync();
                            string message = Encoding.UTF8.GetString(buffer, 0, "Hello".Length);
                            if (message != "Hello")
                            {
                                Debug.WriteLine("HandShake - fail to receive Hello");
                                return false;
                            }
                            // Next step
                            handShakeStep++;
                            Debug.WriteLine("HandShake - received Helo");
                            break;
                        }
                    case 2:
                        {
                            bool bRetValue = await InternalStreamWriteAsync(_nonce);
                            if (!bRetValue)
                            {
                                Debug.WriteLine("HandShake - fail to send nonce");
                                return false;
                            }
                            // Next step
                            handShakeStep++;
                            Debug.WriteLine("HandShake - sended nonce");
                            break;
                        }
                }
            }
            return true;
        }

        private async Task<bool> ConnectClientAsync()
        {
            try
            {
                await client.ConnectAsync(iPEndPoint);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ConnectClientAsync - Exception - {ex.Message}");
                return false;
            }
            return true;
        }

        private async Task<byte[]> InternalStreamReadAsync()
        {
            byte[] buffer = new byte[1_024];
            try
            {
                await using NetworkStream stream = client.GetStream();
                int received = await stream.ReadAsync(buffer, 0, buffer.Length);
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
