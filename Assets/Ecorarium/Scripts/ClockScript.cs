using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    public AudioSource ringring;

    public void TriggerDay()
    {
        GameEventsManager.current.Day();
        ringring.Play();
    }

    public void TriggerNight()
    {
        GameEventsManager.current.Night();
    }
}
