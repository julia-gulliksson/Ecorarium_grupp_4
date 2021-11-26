using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieController : MonoBehaviour
{
    //Growth Controll
    float timePassed;
    float timeBetweenTick = 10.0f;
    public float growthRange;
    public float checkRange;
    public int allowedNeighbours;
    bool isGrowing;
    int matureTime;
    Vector3 growthPoint;

    public GameObject platPrefab;

    public LayerMask veggies, whatisGrowable;

    // Start is called before the first frame update
    void Start()
    {
        matureTime = 0;
        isGrowing = true;
        EventHandler.current.OnTimeTick += TimeTick;
    }

    private void TimeTick()
    {
        print("I heard the tick");
        CheckNeighbours();
    }

    // Update is called once per frame


    private void CheckNeighbours()
    {
        isGrowing = true;
        print("Checking other plants");
        Collider[] neighbours = Physics.OverlapSphere(transform.position, checkRange, veggies);
        if (neighbours.Length < allowedNeighbours) SpurtCheck();

        /*if (!Physics.CheckSphere(transform.position, growthRange, veggies))
        {
            print("No Neighbours");
            SpurtCheck();
        }*/

    }
    private void SpurtCheck()
    {
        print("SpurtCheckComplete");
        float randomZ = Random.Range(-growthRange, growthRange);
        float randomX = Random.Range(-growthRange, growthRange);
        Vector2 randomDirection = new Vector2(randomX, randomZ);

        growthPoint = new Vector3(transform.position.x + randomDirection.x, transform.position.y, transform.position.z + randomDirection.y);
        // walkPoint = walkPoint.normalized;
        if (Physics.Raycast(growthPoint, -transform.up, 2f, whatisGrowable))
            Grow(growthPoint);


    }
    private void Grow(Vector3 growthPoint)
    {
        Instantiate(platPrefab, growthPoint, Quaternion.identity);
    }

    private void OnDestroy()
    {
        EventHandler.current.OnTimeTick -= TimeTick;
    }
}
