using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private void OnEnable()
    {
        GameEventsManager.current.onGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onGameOver -= GameOver;
    }

    private void GameOver()
    {
        // Show game over ui, lock player position
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // Quit 
    }

    public void MainMenu()
    {
        // Go to main menu
    }
}
