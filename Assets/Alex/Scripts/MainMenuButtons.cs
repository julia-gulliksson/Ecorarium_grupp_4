using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Alex
{
    public class MainMenuButtons : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("EcoTerrain 1");
        }

        public void QuitApp()
        {
            Application.Quit();
        }
    }
}
