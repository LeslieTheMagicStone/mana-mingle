using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance => instance;
    private static SceneController instance;

    const int MAIN_MENU_INDEX = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(sceneIndex + 1);
        else Debug.LogWarning("Scene index out of range.");
    }

    public void ReloadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_INDEX);
    }
}
