using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nrOfDays;

    void Start()
    {
        nrOfDays.text = ScoreManager.current.Score.ToString();
    }
}
