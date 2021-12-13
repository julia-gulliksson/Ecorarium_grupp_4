using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace TheLoneHerder
{ 
    public class PauseMenu : MonoBehaviour
    {

        public GameObject pauseMenu;

        public bool activatePauseMenu = true;

        // Start is called before the first frame update
        void Start()
        {
            DisplayPauseMenu();
        }

        public void Restart()
        {
            SceneManager.LoadScene("MainGame");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Lobby");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void MenuPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
                DisplayPauseMenu();
        }

        public void DisplayPauseMenu()
        {
            if (activatePauseMenu)
            {
                pauseMenu.SetActive(false);
                activatePauseMenu = false;
            }
            else if(!activatePauseMenu)
            {
                pauseMenu.SetActive(true);
                activatePauseMenu = true;
            }

        }
    }
}
