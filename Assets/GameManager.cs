using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int keysFound = 3;
    private int lives = 3;
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
    public TMP_Text highScoreText;
    public TMP_Text timeText;
    public TMP_Text jakoscText;
    public TMP_Text endScore;
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

        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "First Stage")
        {
            currentScore = PlayerPrefs.GetInt("CurrentScore", 0);  // Default to 0 if not found
            lives = PlayerPrefs.GetInt("Lives", 3);  // Default to 0 if not found
            enemiesKilled = PlayerPrefs.GetInt("EnemiesKilled", 0);  // Default to 0 if not found
            Debug.Log("score from previous game:" + currentScore);

            livesText.text = "ECTS: " + lives.ToString();
            scoreText.text = currentScore.ToString();
            enemiesDefeated.text = enemiesKilled.ToString();
        }

        pauseMenuCanvas.enabled = false;
        LevelCompletedCanvas.enabled = false;
        OptionsCanvas.enabled = false;

        for (int i = 0; i < keysTab.Length; i++)
            keysTab[i].color = Color.grey;


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

        Debug.Log("Entered LevelCompleted");


        SetGameState(GameState.LEVEL_COMPLETED);

        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "First Stage" && lives > 0)
        {
            keysFound = 0;
            Debug.Log("current score:" + currentScore);
            PlayerPrefs.SetInt("Lives", lives);
            PlayerPrefs.SetInt("CurrentScore", currentScore);
            PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
            SceneManager.LoadScene("Second Stage");
            SetGameState(GameState.GAME);
            return;

        }

        Debug.Log("Not First stage");
        // Check if the current scene is "Second Stage"
        if (currentScene.name == "Second Stage" || lives == 0)
        {

            // Retrieve the high score for "Level1" from PlayerPrefs
            int highScore = PlayerPrefs.GetInt("HighScore_Level1", 0);
            currentScore += 100 * lives;
            // Check if the current score exceeds the saved high score
            Debug.Log("current score:" + currentScore);
            if (currentScore > highScore)
            {
                highScore = currentScore; // Update the high score
                PlayerPrefs.SetInt("HighScore_Level1", highScore); // Save the new high score
            }

            // Update the UI with the current score and high score
            if (endScore != null)
                endScore.text = "Score: " + currentScore;

            if (highScoreText != null)
                highScoreText.text = "High Score: " + highScore;


            PlayerPrefs.SetInt("Lives", 3);
            PlayerPrefs.SetInt("CurrentScore", 0);
            PlayerPrefs.SetInt("EnemiesKilled", 0);
        }



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

    public void UpdatePlayerLives(bool lostLive)
    {
        if (lostLive)
            lives--;
        else
            lives++;


        livesText.text = "ECTS: " + lives.ToString();
        if (lives == 0)
            LevelCompleted();
    }
    public void UpdatePoints(int score)
    {
        currentScore += score;
        scoreText.text = currentScore.ToString();
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
