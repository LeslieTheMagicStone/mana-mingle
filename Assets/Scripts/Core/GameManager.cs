using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance => _instance;
    public bool debugMode => _debugMode;

    [SerializeField] private bool _debugMode;
    private static GameManager _instance;


    private void Awake()
    {
        if (_instance == null) _instance = this;
        else { Debug.LogError("Multiple GameManager instance detected!"); Destroy(gameObject); }
    }
}
