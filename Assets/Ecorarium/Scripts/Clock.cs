using UnityEngine;

public class Clock : MonoBehaviour
{
    AudioSource ringring;
    Animator animator;

    private void Start()
    {
        ringring = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameEventsManager.current.onGameOver += StopAnimation;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onGameOver -= StopAnimation;
    }

    private void StopAnimation()
    {
        // Game is over, disable day/night cycle
        animator.enabled = false;
    }

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
