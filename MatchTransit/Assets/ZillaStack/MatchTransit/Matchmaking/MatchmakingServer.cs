using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Net;

namespace Assets.ZillaStack.MatchTransit.Matchmaking
{
    public class MatchmakingServer : MonoBehaviour, INetEventListener
    {
        private NetManager _server;
        private Dictionary<string, Match> _matches = new Dictionary<string, Match>();

        void Start()
        {
            _server = new NetManager(this);
            _server.Start(9050);
            Debug.Log("Server started on port 9050");
        }

        void Update()
        {
            _server.PollEvents();
        }

        void OnDestroy()
        {
            _server.Stop();
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.Accept();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Debug.Log("Client connected");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log("Client disconnected");
            // Remove player from matches
            foreach (var match in _matches.Values)
            {
                match.Players.Remove(peer);
            }
        }

        public void OnNetworkError(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
            Debug.Log("Network error: " + socketError);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var message = JsonUtility.FromJson<Message>(reader.GetString());

            switch (message.MessageType)
            {
                case MessageType.CreateMatch:
                    CreateMatch(peer, message.GameType);
                    break;
                case MessageType.ListMatches:
                    ListMatches(peer);
                    break;
                case MessageType.JoinMatch:
                    JoinMatch(peer, message.MatchID);
                    break;
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Debug.Log("Received unconnected message");
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            OnNetworkReceive(peer, reader, deliveryMethod);
        }

        private void CreateMatch(NetPeer peer, string gameType)
        {
            var matchID = System.Guid.NewGuid().ToString();
            var match = new Match(matchID, gameType);
            match.Players.Add(peer);
            _matches.Add(matchID, match);

            Debug.Log($"Match created: {matchID} with game type: {gameType}");
        }

        private void ListMatches(NetPeer peer)
        {
            var matchList = new List<MatchInfo>();

            foreach (var match in _matches.Values)
            {
                matchList.Add(new MatchInfo { MatchID = match.MatchID, GameType = match.GameType, PlayerCount = match.Players.Count });
            }

            var matchListMessage = new MatchListMessage { Matches = matchList };
            var writer = new NetDataWriter();
            writer.Put(JsonUtility.ToJson(matchListMessage));
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        private void JoinMatch(NetPeer peer, string matchID)
        {
            if (_matches.ContainsKey(matchID))
            {
                _matches[matchID].Players.Add(peer);
                Debug.Log($"Player joined match: {matchID}");
            }
            else
            {
                Debug.Log($"Match not found: {matchID}");
            }
        }
    }
}
