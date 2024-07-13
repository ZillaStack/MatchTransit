using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.ZillaStack.MatchTransit.Matchmaking
{
    internal class MatchmakingClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private bool running;

        public string serverIP = "127.0.0.1"; // Change this to your server's IP address
        public int port = 5500;

        public void ConnectToServer(string serverIp, int serverPort)
        {
            this.serverIP = serverIp;
            port = serverPort;
            try
            {
                client = new TcpClient(serverIP, port);
                stream = client.GetStream();
                running = true;

                receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                Debug.Log($"Connected to server on {serverIp}:{port}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Connection error: {e.Message}");
            }
        }

        private void ReceiveData()
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (running)
                {
                    if (stream.DataAvailable)
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Debug.Log($"Received: {message}");
                        }
                    }

                    Thread.Sleep(10); // Reduce CPU usage
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Receive error: {e.Message}");
            }
        }

        public void Disconnect()
        {
            running = false;
            receiveThread?.Join();
            stream?.Close();
            client?.Close();
            Debug.Log("Disconnected from server");
        }
    }
}
