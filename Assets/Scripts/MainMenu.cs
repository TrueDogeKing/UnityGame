using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor; // Required for Editor-specific functionality
#endif

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas OptionsCanvas;
    public Canvas MenuCanvas;

    void Start()
    {
        OptionsCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Options()
    {
        MenuCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }
    public void MainMenuButton()
    {
        MenuCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }
    public void Onlevel1ButtonPressed()
    {
        SceneManager.LoadScene("First Stage");
    }

    public void OnExitToDesktopButtonPressed()
    {
    #if UNITY_EDITOR
            // Exit play mode in the Editor
            EditorApplication.isPlaying = false;
    #else
            // Quit the application
            Application.Quit();
    #endif

    }
}
