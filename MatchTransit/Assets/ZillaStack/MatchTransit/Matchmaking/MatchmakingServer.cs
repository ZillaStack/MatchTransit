using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.ZillaStack.MatchTransit.Matchmaking
{
    internal class MatchmakingServer
    {
        private TcpListener server;
        private Thread serverThread;
        private bool running;

        public int port = 5500;

        /// <summary>
        /// Starts the server, could be called from MonoBehaviour.Start()
        /// </summary>
        public void StartServer(int serverPort)
        {
            port = serverPort;

            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                running = true;

                serverThread = new Thread(ServerLoop);
                serverThread.IsBackground = true;
                serverThread.Start();

                Debug.Log($"Server started on port {port}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Server error: {e.Message}");
            }
        }

        private void ServerLoop()
        {
            try
            {
                while (running)
                {
                    if (server.Pending())
                    {
                        TcpClient client = server.AcceptTcpClient();
                        Debug.Log("Client connected");

                        Thread clientThread = new Thread(() => HandleClient(client));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }

                    Thread.Sleep(10); // Reduce CPU usage
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Server loop error: {e.Message}");
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                try
                {
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Debug.Log($"Received: {message}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Client handling error: {e.Message}");
                }
            }

            Debug.Log("Client disconnected");
            client.Close();
        }

        /// <summary>
        /// Stops the server, could be called from MonoBehaviour.OnApplicationQuit()
        /// </summary>
        public void StopServer()
        {
            running = false;
            server?.Stop();
            serverThread?.Join();
            Debug.Log("Server stopped");
        }
    }
}
