using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignController : MonoBehaviour
{
    public int score = 0;
    private TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        GameEventsManager.current.OnDay += ScoreDay;
    }

    private void ScoreDay()
    {
        score++;
        text.text = score.ToString();
    }
}
