using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;

namespace Assets.ZillaStack.MatchTransit.Matchmaking
{
    public class MatchmakingClient : MonoBehaviour, INetEventListener
    {
        private NetManager _client;
        private NetPeer _serverPeer;

        void Start()
        {
            _client = new NetManager(this);
            _client.Start();
            _client.Connect("localhost", 9050, "SomeConnectionKey");
        }

        void Update()
        {
            _client.PollEvents();
        }

        void OnDestroy()
        {
            _client.Stop();
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Debug.Log("Connected to server");
            _serverPeer = peer;
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log("Disconnected from server");
        }

        public void OnNetworkError(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
            Debug.Log("Network error: " + socketError);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var message = JsonUtility.FromJson<Message>(reader.GetString());

            if (message.MessageType == MessageType.MatchList)
            {
                Debug.Log("Match list received: " + message.MatchID);
            }
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            OnNetworkReceive(peer, reader, deliveryMethod);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Debug.Log("Received unconnected message");
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void CreateMatch(string gameType)
        {
            var message = new Message
            {
                MessageType = MessageType.CreateMatch,
                GameType = gameType
            };
            var writer = new NetDataWriter();
            writer.Put(JsonUtility.ToJson(message));
            _serverPeer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        public void ListMatches()
        {
            var message = new Message
            {
                MessageType = MessageType.ListMatches
            };
            var writer = new NetDataWriter();
            writer.Put(JsonUtility.ToJson(message));
            _serverPeer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        public void JoinMatch(string matchID)
        {
            var message = new Message
            {
                MessageType = MessageType.JoinMatch,
                MatchID = matchID
            };
            var writer = new NetDataWriter();
            writer.Put(JsonUtility.ToJson(message));
            _serverPeer.Send(writer, DeliveryMethod.ReliableOrdered);
        }
    }
}
