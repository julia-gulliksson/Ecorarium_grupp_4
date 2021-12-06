using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    public void TriggerDay()
    {
        GameEventsManager.current.Day();
    }

    public void TriggerNight()
    {
        GameEventsManager.current.Night();
    }
}
