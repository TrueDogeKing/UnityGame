using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int keysFound = 0;

    public TMP_Text livesText;
    // Enum for game states with custom names for Unity Inspector
    public enum GameState
    {
        [InspectorName("Gameplay")] GAME,
        [InspectorName("Pause")] PAUSE_MENU,
        [InspectorName("Level completed or in progress")] LEVEL_COMPLETED,
        [InspectorName("Options")] OPTIONS,
    }
    // Enum for game modes
    public enum GameMode
    {
        [InspectorName("Platformer")] PLATFORMER,
        [InspectorName("Boss")] BOSS,
    }
    // Enum for boss fight states
    public enum BossFightState
    {
        [InspectorName("Aiming Angle")] AIM_ANGLE,
        [InspectorName("Aiming Force")] AIM_FORCE,
        [InspectorName("Throwing")] THROW,
        [InspectorName("Drinking Beer")] DRINKING,
    }

    public Canvas gameCanvas;
    public Image[] keysTab;

    //tmp enemies defeted
    public TMP_Text enemiesDefeated;
    private int enemiesKilled = 0;

    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text jakoscText;
    public float timer = 0f;

    public Canvas pauseMenuCanvas;
    public Canvas LevelCompletedCanvas;
    public Canvas OptionsCanvas;
    public int currentScore = 0;
    // Public variable to track the current game state
    public GameState currentGameState = GameState.PAUSE_MENU;
    public GameMode currentGameMode = GameMode.PLATFORMER;

    // Variables for boss fight
    public bool isPlayersTurn = true;
    public BossFightState bossFightState = BossFightState.AIM_ANGLE;

    // Singleton instance of GameManager
    public static GameManager instance;
    private Scene currentScene;
    // Awake method to ensure only one instance of GameManager exists
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicated Game Manager", gameObject);
            Destroy(gameObject);
        }

        pauseMenuCanvas.enabled = false;
        LevelCompletedCanvas.enabled = false;
        OptionsCanvas.enabled = false;

        for (int i = 0; i < keysTab.Length; i++)
            keysTab[i].color = Color.grey;

        SetVolume(1);
    }

    // Method to set the game state
    public void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        pauseMenuCanvas.enabled = (currentGameState == GameState.PAUSE_MENU);
        gameCanvas.enabled = (currentGameState == GameState.GAME);
        LevelCompletedCanvas.enabled = (currentGameState == GameState.LEVEL_COMPLETED);
        OptionsCanvas.enabled = (currentGameState == GameState.OPTIONS);
    }

    // Public methods to set specific game states
    public void PauseMenu()
    {
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        SetGameState(GameState.PAUSE_MENU);
    }

    public void InGame()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SetGameState(GameState.GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }

    public void Options()
    {
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        SetGameState(GameState.OPTIONS);
    }

    public void QualityUp()
    {
        QualitySettings.IncreaseLevel();
        jakoscText.text = "Jakosc: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void QualityDown()
    {
        QualitySettings.DecreaseLevel();
        jakoscText.text = "Jakosc: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void SetVolume(float volume)
    {
        Debug.Log("volume:" + volume);
        AudioListener.volume = volume;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GAME)
                PauseMenu();
            else if (currentGameState == GameState.PAUSE_MENU)
                InGame();
        }


        timer += Time.deltaTime;

        if (timeText)
        {
            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(timer / 60); // Divide total seconds by 60 to get minutes
            int seconds = Mathf.FloorToInt(timer % 60); // Use modulo to get remaining seconds

            // Format the time in "minutes:seconds" format and display it
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void AddKeys(int id)
    {
        if (keysTab[id] != null)
        {
            if (id == 0)
            {
                keysTab[id].color = Color.white;
            }
            else if (id == 1)
            {
                keysTab[id].color = Color.red;
            }
            else if (id == 2)
            {
                keysTab[id].color = new Color(1f, 1f, 0f);
            }
        }
        else
        {
            Debug.LogWarning($"keysTab[{keysFound}] is null!");
        }
        keysFound++;
    }

    public void UpdatePlayerLives(int newLives)
    {
        livesText.text = "ECTS: " + newLives.ToString();
    }
    public void UpdatePoints(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateEnemies()
    {
        enemiesKilled++;
        enemiesDefeated.text = enemiesKilled.ToString();
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1.0f; // Reset time scale to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

}
