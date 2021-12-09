using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLoneHerder
{
    public class DayNightSounds : MonoBehaviour
    {
        public AudioSource DayAudio;
        public AudioSource NightAudio;

        // Start is called before the first frame update
        void Start()
        {
            GameEventsManager.current.OnDay += DaySound;
            GameEventsManager.current.OnNight += NightSound;
            DaySound();
        }

        private void DaySound()
        {
            DayAudio.Play();
            NightAudio.Stop();
        }

        private void NightSound()
        {
            NightAudio.Play();
            DayAudio.Stop();
        }

    }
}