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
    public GameState gameState => _gameState;
    public Dictionary<ulong, PlayerInfo> playerInfos { get; private set; }
    public UnityEvent onStartGame;

    [SerializeField] private bool _debugMode;
    private static GameManager _instance;
    private GameState _gameState;

    const string GAME_SCENE_NAME = "DemoScene";


    private void Start()
    {
        if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); }
        else { Debug.LogError("Multiple GameManager instance detected!"); Destroy(gameObject); }

        playerInfos = new();

        SceneManager.LoadScene(1);

        NetworkManager.OnClientStarted += AddOnLoadEventComplete;
    }

    private void Update()
    {

        if (gameState == GameState.Normal)
        {
        }
    }

    public void StartGame(Dictionary<ulong, PlayerInfo> playerInfos)
    {
        this.playerInfos = playerInfos;
    }

    public void SetGameState(GameState state)
    {
        if (state == GameState.Normal)
        {
        }
        else if (state == GameState.GameOver)
        {
        }
        _gameState = state;
    }

    private void AddOnLoadEventComplete()
    {
        NetworkManager.SceneManager.OnLoadEventCompleted += OnLoadEventComplete;
    }

    private void OnLoadEventComplete(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (sceneName == GAME_SCENE_NAME)
        {
            onStartGame.Invoke();
        }
    }
}
