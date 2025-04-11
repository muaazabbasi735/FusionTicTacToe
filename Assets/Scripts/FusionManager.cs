using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner runner;
    [SerializeField] private bool isProvideInput = true;

    public GameObject gameManagerPrefab;
    public GameManager gameManagerScript; 

    public static FusionManager instance;

    public TMP_Text statusMessage;
    public Transform cellsParent;

    private void Awake()
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        statusMessage.text = "Connecting...";
        NetworkRunner.CloudConnectionLost += OnCloudConnectionLost;
        StartGame();
    }

    async void StartGame()
    {
        runner.ProvideInput = isProvideInput;
        StartGameArgs args = new StartGameArgs();
        args.GameMode = GameMode.Shared;
        args.SessionName = "";
        args.PlayerCount = 2;
        args.Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        args.SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        await runner.StartGame(args);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("We are connected to the server!");
        statusMessage.text = "Connected to the server.";

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        statusMessage.text = "Connection to the server failed";

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        statusMessage.text = "Disconnected from the server.";
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("A player has joined with PlayerRef:"+player);
        if (runner.SessionInfo.PlayerCount > 1)
        {
            if (runner.LocalPlayer.PlayerId == 1)
            {
                NetworkObject gm = runner.Spawn(gameManagerPrefab);
                gameManagerScript = gm.GetComponent<GameManager>();
                gameManagerScript.startGame();
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("A player has Left with PlayerRef:" + player);

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    private void OnCloudConnectionLost(NetworkRunner runner, ShutdownReason reason, bool reconnecting)
    {
        statusMessage.text = "C";

    }
}