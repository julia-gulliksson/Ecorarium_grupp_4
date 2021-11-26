using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class EventHandler : MonoBehaviour
{
    public static EventHandler current;
    public bool isGameActive;
    void Awake()
    {
        isGameActive = true;
        current = this;
        StartCoroutine(TickTock());
        print("I'm awake!");
        
    }

    public event Action OnTimeTick;
    public event Action<Vector3, int> OnBreedingCall;

    public void BreedingCall(Vector3 position, int gender)
    {
        if (OnBreedingCall != null)
        {
            OnBreedingCall(position, gender);
            
        }
    }

    public void TimeTick()
    {
        if (OnTimeTick != null)
        {
            OnTimeTick();
            print("Working function in EventHandler");
        }
    }

    IEnumerator  TickTock()
    {
        while (isGameActive)
        {
            print("Tick");
            yield return new WaitForSeconds(5);
            print("Tock");
            current.TimeTick();
        }
    }

   
}
