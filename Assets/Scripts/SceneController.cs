using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : NetworkBehaviour
{
    public static SceneController instance { get; private set; }
    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
    public void LoadScene(string name)
    {
        NetworkManager.SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
