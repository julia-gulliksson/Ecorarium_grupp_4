using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int SheepSpawn = 8;
    public Text SheepText;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Easy()
    {
        int SheepSpawn = 12;
    }

    public void Medium()
    {
        int SheepSpawn = 8;
    }

    public void Hard()
    {
        int SheepSpawn = 4;
    }

    void Update()
    {
        SheepText.text = SheepSpawn.ToString();
    }

}
