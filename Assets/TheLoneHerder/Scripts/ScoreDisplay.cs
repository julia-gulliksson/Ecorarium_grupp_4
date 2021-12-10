using UnityEngine;
using TMPro;

namespace TheLoneHerder
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nrOfDays;

        void Start()
        {
            nrOfDays.text = ScoreManager.current.Score.ToString();
        }
    }
}