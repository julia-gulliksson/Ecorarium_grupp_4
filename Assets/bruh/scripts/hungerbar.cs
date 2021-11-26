using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hungerbar : MonoBehaviour
{
    private bool Alive = true;
    private Slider slider;

    public float FillSpeed = 0.1f; 
    public float targetHunger = 0;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DecreaseHunger(-0.99f);
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value > targetHunger)
        slider.value += FillSpeed * -Time.deltaTime;

        if (slider.value < 0.01 && Alive)
        {
            Debug.Log("Rip");
            Alive = false;
        }
    }

    public void DecreaseHunger(float newHunger)
    {
        targetHunger = slider.value + newHunger;
    }
}
