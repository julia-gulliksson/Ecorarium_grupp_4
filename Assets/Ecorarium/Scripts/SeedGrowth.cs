using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGrowth : MonoBehaviour
{
    public Rigidbody CabbageHarvestable;
    public Transform spot;
    public int growth = 0;
    bool planted = false;

    // Start is called before the first frame update
    void Start()
    {
        GameEventsManager.current.OnDay += GrowthProgression;
    }

    // Update is called once per frame
    void Update()
    {
        if (planted && growth < 2)
        {
            gameObject.transform.GetChild(growth).gameObject.SetActive(true);
            gameObject.transform.GetChild(growth - 1).gameObject.SetActive(false);
        }
        else if (planted && growth == 2)
        {
            Finish();
        }
    }

    void GrowthProgression()
    {
        if (planted == true)
        {
            growth++;
        }
    }

    private void Finish()
    {
        Instantiate (CabbageHarvestable, spot.position, spot.rotation);
        gameObject.transform.GetChild(growth - 1).gameObject.SetActive(false);
        growth = 0;
        planted = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "CabbageSeed")
        {
            planted = true;
        }
    }

}