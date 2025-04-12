using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;
using DG.Tweening;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner runner;
    [SerializeField] private bool isProvideInput = true;
    public static FusionManager instance;
    public Transform roomsParent;
    public GameObject roomsPrefab;
    public Transform connectionCanvas;
    public string roomName;
    private bool firstLoad = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        SceneManager.sceneLoaded+=SceneLoaded;
    }

    private void SceneLoaded(Scene sArg, LoadSceneMode sceneMode)
    {
        if(!firstLoad)
        {
            if(sArg.buildIndex == 0)
            {
                JoinLobby();
            }
        }
        else
        {
            firstLoad = false;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        NetworkRunner.CloudConnectionLost += OnCloudConnectionLost;
        JoinLobby();
    }

    public void JoinLobby()
    {
        if(runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }
        runner.JoinSessionLobby(SessionLobby.Shared);
    }

    public void JoinSession(string sessionName)
    {
        roomName = sessionName;
        StartGame();
    }

    public void CreateSession()
    {
        roomName = "Room-" + Random.Range(1000, 9999);
        StartGame();
    }

    async void StartGame()
    {
        runner.ProvideInput = isProvideInput;
        StartGameArgs args = new StartGameArgs();
        args.GameMode = GameMode.AutoHostOrClient;
        args.SessionName = roomName;
        args.PlayerCount = 2;
        args.Scene = SceneRef.FromIndex(1);
        args.SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        await runner.StartGame(args);
    }

   
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("We are connected to the server!");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        FindObjectOfType<GameManager>().statusMsg.text = "The player has Left!";

    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("A player has joined with PlayerRef:" + player);
        if (runner.SessionInfo.PlayerCount > 1)
        {
            if (runner.LocalPlayer.PlayerId == 1)
            {
                FindObjectOfType<GameManager>().startGame();
            }
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        FindObjectOfType<GameManager>().statusMsg.text = "A player has Left!";
        runner.Shutdown(false, ShutdownReason.Ok, true);
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (roomsParent == null)
        {
            roomsParent = GameObject.Find("Content").transform;
        }

        foreach (Transform child in roomsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (SessionInfo sessionInfo in sessionList)
        {
            if (sessionInfo.IsVisible)
            {

                GameObject sessionObj = Instantiate(roomsPrefab, roomsParent);
                sessionObj.transform.localScale = Vector3.zero;
                sessionObj.transform.DOScale(Vector3.one, 1f);
                RoomPrefabScript roomPrefabScript = sessionObj.GetComponent<RoomPrefabScript>();
                roomPrefabScript.roomName.text = sessionInfo.Name;
                string playerCount = sessionInfo.PlayerCount + "/" + sessionInfo.MaxPlayers;
                roomPrefabScript.playerCount.text = playerCount;

                if (!sessionInfo.IsOpen || sessionInfo.PlayerCount > 1)
                {
                    roomPrefabScript.joinButton.interactable = false;
                }
            }
        }

    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        StartCoroutine(ReturnToLobby());
    }
    private IEnumerator ReturnToLobby()
    {
        Destroy(runner);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(0);
    }
    private void OnCloudConnectionLost(NetworkRunner runner, ShutdownReason reason, bool reconnecting)
    {
        Debug.Log($"Cloud Connection Lost: {reason} (Reconnecting: {reconnecting})");

        FindObjectOfType<GameManager>().statusMsg.text = "Disconnected from the cloud, Attempting reconnection now!";

        if (!reconnecting)
        {
            // Handle scenarios where reconnection is not possible
            // e.g., notify the user, attempt manual reconnection, etc.
            runner.Shutdown(false, ShutdownReason.Ok, true);
        }
        else
        {
            // Wait for automatic reconnection
            StartCoroutine(WaitForReconnection(runner));
        }

    }
    private IEnumerator WaitForReconnection(NetworkRunner runner)
    {
        yield return new WaitUntil(() => runner.IsInSession);
        Debug.Log("Reconnected to the Cloud!");
        FindObjectOfType<GameManager>().statusMsg.text = "Reconnected to the Cloud!";
    }



    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
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
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Scene Loaded!");
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

}