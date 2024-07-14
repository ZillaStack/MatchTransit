using Assets.ZillaStack.MatchTransit;
using Assets.ZillaStack.MatchTransit.Matchmaking;
using UnityEngine;

public class MatchTransitBehaviour : MonoBehaviour
{
    public string ServerIp = "127.0.0.1";
    public int ServerPort = 5500;
    public bool IsMatchmakerServer;
    public bool IsMatchmakerClient;

    private MatchmakingServer _matchmakingServer;
    private MatchmakingClient _matchmakingClient;

    void Start()
    {
        MatchTransitServices.ConfigureServices();

        Debug.Log("Starting MatchTransitBehaviour...");

        //if (IsMatchmakerServer)
        //{
        //    Debug.Log("Starting IsMatchmakerServer...");
        //    _matchmakingServer = MatchTransitServices.Get<MatchmakingServer>();
        //    _matchmakingServer.StartServer(ServerPort);
        //}

        //if (IsMatchmakerClient)
        //{
        //    Debug.Log("Starting IsMatchmakerClient...");
        //    _matchmakingClient = MatchTransitServices.Get<MatchmakingClient>();
        //    _matchmakingClient.ConnectToServer(ServerIp, ServerPort);
        //}
    }

    void OnApplicationQuit()
    {
        //_matchmakingServer?.StopServer();
        //_matchmakingClient?.Disconnect();
    }
}
