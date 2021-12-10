using UnityEngine;
using TMPro;

namespace TheLoneHerder
{
    public class SignController : MonoBehaviour
    {
        private TMP_Text text;

        void Start()
        {
            text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            GameEventsManager.current.onScoreChanged += UpdateScore;
        }

        private void OnDisable()
        {
            GameEventsManager.current.onScoreChanged -= UpdateScore;
        }

        private void UpdateScore()
        {
            text.text = ScoreManager.current.Score.ToString();
        }
    }
}