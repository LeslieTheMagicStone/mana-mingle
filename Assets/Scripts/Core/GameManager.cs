using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private bool _debugMode;
    private static GameManager _instance;
    private GameState _gameState;

    public Dictionary<ulong, PlayerInfo> playerInfos { get; private set; }

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else { Debug.LogError("Multiple GameManager instance detected!"); Destroy(gameObject); }

        playerInfos = new();
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

    public void LoadScene(string name)
    {
        NetworkManager.SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
