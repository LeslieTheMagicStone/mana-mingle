using UnityEngine;

public enum GameState
{
    Picking,
    Paused,
    Fighting,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance => _instance;
    public bool debugMode => _debugMode;
    public GameState gameState => _gameState;
    public float stateTimer => _stateTimer;
    public float stateTimerMax => _stateTimerMax;

    [SerializeField] private bool _debugMode;
    private static GameManager _instance;
    private GameState _gameState;
    private float _stateTimer;
    private float _stateTimerMax;

    const float PICKING_STATE_TIME = 60f;
    const float FIGHTING_STATE_TIME = 60f;


    private void Awake()
    {
        if (_instance == null) _instance = this;
        else { Debug.LogError("Multiple GameManager instance detected!"); Destroy(gameObject); }

        _stateTimer = 0f;
    }

    private void Update()
    {
        if (gameState == GameState.Picking)
        {
            _stateTimer -= Time.deltaTime;
            if (_stateTimer < 0f)
            {
                SetGameState(GameState.Fighting);
            }
        }

        if (gameState == GameState.Fighting)
        {
            _stateTimer -= Time.deltaTime;
            if (_stateTimer < 0f)
            {
                SetGameState(GameState.GameOver);
            }
        }
    }

    public void SetGameState(GameState state)
    {
        if (state == GameState.Picking)
        {
            _stateTimer = PICKING_STATE_TIME;
            _stateTimerMax = PICKING_STATE_TIME;
        }
        else if (state == GameState.Paused)
        {
        }
        else if (state == GameState.Fighting)
        {
            _stateTimer = FIGHTING_STATE_TIME;
            _stateTimerMax = FIGHTING_STATE_TIME;
        }
        else if (state == GameState.GameOver)
        {
        }
        _gameState = state;
    }
}
