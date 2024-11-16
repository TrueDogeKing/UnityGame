using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class GameManager : MonoBehaviour
{
    private int keysFound=0;

    public TMP_Text lifesText;
    // Enum for game states with custom names for Unity Inspector
    public enum GameState
    {
        [InspectorName("Gameplay")] GAME,
        [InspectorName("Pause")] PAUSE_MENU,
        [InspectorName("Level completed or in progress")] LEVEL_COMPLETED
    }
    public Canvas gameCanvas;
    public Image[] keysTab;

    //tmp enemies defeted
    public TMP_Text enemiesDefeated;
    private int enemiesKilled = 0;

    public TMP_Text scoreText;
    public TMP_Text timeText;
    float timer = 0f;


    // Public variable to track the current game state
    public GameState currentGameState = GameState.PAUSE_MENU;

    // Singleton instance of GameManager
    public static GameManager instance;

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

        for (int i = 0; i< 3; i++)
            keysTab[i].color=Color.grey;

    }

    // Method to set the game state
    public void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        //gameCanvas.enabled;

    }

    // Public methods to set specific game states
    public void PauseMenu()
    {
        Time.timeScale = 0.0f;
        AudioListener.pause = false;
        SetGameState(GameState.PAUSE_MENU);
    }

    public void InGame()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = true;
        SetGameState(GameState.GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
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

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(timer / 60); // Divide total seconds by 60 to get minutes
        int seconds = Mathf.FloorToInt(timer % 60); // Use modulo to get remaining seconds

        // Format the time in "minutes:seconds" format and display it
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

    public void UpdatePlayerLifes(int newLifes)
    {
        lifesText.text = newLifes.ToString();
        lifesText.text += "-ECTS";
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
}
