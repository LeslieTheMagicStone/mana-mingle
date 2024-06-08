using UnityEngine;

public enum GameState
{
    Normal,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance => _instance;
    public bool debugMode => _debugMode;
    public GameState gameState => _gameState;

    [SerializeField] private bool _debugMode;
    private static GameManager _instance;
    private GameState _gameState;

    const float PICKING_STATE_TIME = 60f;
    const float FIGHTING_STATE_TIME = 60f;


    private void Awake()
    {
        if (_instance == null) _instance = this;
        else { Debug.LogError("Multiple GameManager instance detected!"); Destroy(gameObject); }

    }

    private void Update()
    {

        if (gameState == GameState.Normal)
        {
        }
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
}
