using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    private int place = 4;
    private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                gameObject.transform.GetChild(place).gameObject.SetActive(true);
            }
        }

    private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                gameObject.transform.GetChild(place).gameObject.SetActive(false);
            }
        }
}
