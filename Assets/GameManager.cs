using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Enum for game states with custom names for Unity Inspector
    public enum GameState
    {
        [InspectorName("Gameplay")] GAME,
        [InspectorName("Pause")] PAUSE_MENU,
        [InspectorName("Level completed or in progress")] LEVEL_COMPLETED
    }

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
    }

    // Method to set the game state
    public void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
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
    }
}
