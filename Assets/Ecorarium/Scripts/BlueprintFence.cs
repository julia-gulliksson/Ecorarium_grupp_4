using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintFence : MonoBehaviour
{
    RaycastHit hit;
    Vector3 movePoint;

    Transform childeTransform;
    bool adjustRotationOnce;

    public LayerMask placeable;
    public float speed = 720;
    public GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {
        adjustRotationOnce = true;
        childeTransform = transform.GetChild(0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 50000.0f, placeable))
        {
            if(hit.transform.CompareTag("Ground"))
                transform.position = hit.point;
            if (hit.transform.CompareTag("Fence"))
                SnapFence();
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000.0f, placeable))
        {
            if (hit.transform.CompareTag("Ground"))
                Unsnap();
            if (hit.transform.CompareTag("Fence"))
                SnapFence();
        }
        transform.Rotate(Vector3.up * speed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);
        if (Input.GetMouseButton(0))
        {
            Instantiate(prefab, childeTransform.position, childeTransform.rotation);
            Destroy(gameObject);
        }
    }

    private void SnapFence()
    {
        Transform snapPointOne = hit.transform.GetChild(0);
        Transform snapPointTwo = hit.transform.GetChild(1);
        float distanceOne = Vector3.Distance(hit.point, snapPointOne.position);
        float distanceTwo = Vector3.Distance(hit.point, snapPointTwo.position);
        if (adjustRotationOnce)
        {
            transform.rotation = hit.transform.rotation;
            adjustRotationOnce = false;
        }

        print("1: "+snapPointOne.name + "\n2: " + snapPointTwo.name);
        if (distanceOne < distanceTwo)
        {
            transform.position = snapPointOne.position;
            childeTransform.localPosition = new Vector3(2.5f, 0.0f, 0f);
            
        }
        else if(distanceOne > distanceTwo)
        {
            transform.position = snapPointTwo.position;
            childeTransform.localPosition = new Vector3(-2.5f, 0.0f, 0f);
            
        }
    }
    private void Unsnap()
    {
        childeTransform.localPosition = new Vector3(0, 0.5f, 0f);
        transform.position = hit.point;
        adjustRotationOnce = true;
    }
}
