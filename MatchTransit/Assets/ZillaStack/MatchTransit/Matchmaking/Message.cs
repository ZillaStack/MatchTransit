namespace Assets.ZillaStack.MatchTransit.Matchmaking
{
    [System.Serializable]
    public class Message
    {
        public MessageType MessageType { get; set; }
        public string MatchID { get; set; }
        public string GameType { get; set; }
    }
}
