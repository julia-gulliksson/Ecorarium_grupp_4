using UnityEngine;

namespace TheLoneHerder
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager current;

        public int Score { get; private set; }

        private void Awake()
        {
            current = this;
            Score = 0;
        }

        private void OnEnable()
        {
            GameEventsManager.current.OnDay += IncrementScore;
        }

        private void OnDisable()
        {
            GameEventsManager.current.OnDay -= IncrementScore;
        }

        public void IncrementScore()
        {
            Score++;
            GameEventsManager.current.ScoreChanged();
        }
    }
}