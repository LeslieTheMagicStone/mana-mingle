using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState
{
    Normal,
    GameOver,
}

public class GameManager : NetworkBehaviour
{
    public static GameManager instance => _instance;
    public bool debugMode => _debugMode;
    public Dictionary<ulong, PlayerInfo> playerInfos { get; private set; }
    public UnityEvent onStartGame;
    public string joinCode;

    [SerializeField] private bool _debugMode;
    private static GameManager _instance;

    private void Start()
    {
        if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); }
        else { Debug.LogError("Multiple GameManager instance detected!"); Destroy(gameObject); }

        playerInfos = new();

        SceneManager.LoadScene(1);

        NetworkManager.OnClientStarted += AddOnLoadEventComplete;
    }

    public void StartGame(Dictionary<ulong, PlayerInfo> playerInfos)
    {
        this.playerInfos = playerInfos;
    }

    private void AddOnLoadEventComplete()
    {
        NetworkManager.SceneManager.OnLoadEventCompleted += OnLoadEventComplete;
    }

    private void OnLoadEventComplete(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (sceneName == Scenes.GameScene.ToString())
        {
            onStartGame.Invoke();
        }
    }
}
