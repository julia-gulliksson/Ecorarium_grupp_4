using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheLoneHerder
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Canvas gameOverUI;
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
            gameOverUI.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void MainMenu()
        {
            // Go to main menu
        }
    }
}