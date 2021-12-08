using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHandler : MonoBehaviour
{
    public float speed;

    void Start()
    {
        StartCoroutine(CalcSpeed());
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (speed > 1 && !collider.gameObject.CompareTag("Sheep"))
        {
            // Call the Damage function if the trigger collider enherits IDestroyable (is a wolf)
            IDestroyable destroyable = collider.gameObject.GetComponent<IDestroyable>();
            destroyable?.Damage();
        }
    }

    IEnumerator CalcSpeed()
    {
        bool isPlaying = true;

        while (isPlaying)
        {
            Vector3 prevPos = transform.position;

            yield return new WaitForFixedUpdate();

            speed = Mathf.RoundToInt(Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime);
        }
    }
}
