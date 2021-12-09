using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHandler : MonoBehaviour
{
    public float speed;
    private RaycastHit hit;
    [SerializeField] LayerMask layer;
    [SerializeField] GameObject effectPrefab;

    void Start()
    {
        StartCoroutine(CalcSpeed());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (speed > 1 && !collider.gameObject.CompareTag("Sheep"))
        {
            // Call the Damage function if the trigger collider enherits IDestroyable (is a wolf)
            IDestroyable destroyable = collider.gameObject.GetComponent<IDestroyable>();
            destroyable?.Damage();
            if(Physics.Raycast(transform.GetChild(0).transform.position, transform.up, out hit, transform.localScale.y, layer))
            {
                Instantiate(effectPrefab, hit.point, Quaternion.identity);
            }
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
