using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Alex
{
    public class GameOverButtons : MonoBehaviour
    {

        public void PlayAgain()
        {
            SceneManager.LoadScene("EcoTerrain 1");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Scen1 1");
        }

        public void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
