using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLoneHerder {
    public class CampfireController : MonoBehaviour
    {
        GameObject fire;
        private void Start()
        {
            fire = transform.GetChild(0).gameObject;
            GameEventsManager.current.OnNight += EgniteFire;
            GameEventsManager.current.OnDay += ExtinguishFire;
        }
        private void OnDisable()
        {
            GameEventsManager.current.OnNight -= EgniteFire;
            GameEventsManager.current.OnDay -= ExtinguishFire;
        }
        private void ExtinguishFire()
        {
            fire.SetActive(false);
        }

        private void EgniteFire()
        {
            fire.SetActive(true);
        }
    }
}