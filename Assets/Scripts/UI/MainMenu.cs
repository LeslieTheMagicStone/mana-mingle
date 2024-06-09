using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start the game
    public void StartGame()
    {
        // Load the game scene (replace "GameScene" with your game scene name)
        SceneManager.LoadScene("SampleScene");
    }

    // Quit the game
    public void QuitGame()
    {
        // If running in the Unity editor, stop playing the scene
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a built application, quit the application
        Application.Quit();
#endif
    }
}
