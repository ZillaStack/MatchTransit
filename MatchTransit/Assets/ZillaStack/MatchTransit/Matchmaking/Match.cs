using LiteNetLib;
using System.Collections.Generic;

namespace Assets.ZillaStack.MatchTransit.Matchmaking
{
    public class Match
    {
        public string MatchID { get; set; }
        public string GameType { get; set; }
        public List<NetPeer> Players { get; set; }

        public Match(string matchID, string gameType)
        {
            MatchID = matchID;
            GameType = gameType;
            Players = new List<NetPeer>();
        }
    }
}
