using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Init,
    MainMenu,
    Lobby,
    GameScene,
}

public class SceneController : NetworkBehaviour
{
    public static SceneController instance { get; private set; }
    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
    public void LoadScene(Scenes scene)
    {
        NetworkManager.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
    }
}
