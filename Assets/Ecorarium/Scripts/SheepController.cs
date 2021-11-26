using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SheepController : MonoBehaviour
{
    public List<GameObject> sheepHerd;
    public List<GameObject> fences;
    public static SheepController sC;
    public GameObject sheepPrefab;
    public GameObject fencePrefab;
    public GameObject bluePrintPrefab;
    public int fenceLevel = 4;

    private float fenceWidth;
    private AudioSource baa;

    // Start is called before the first frame update
    void Awake()
    {
        sC = this;

    }
    private void Start()
    {
        fenceWidth = fencePrefab.transform.localScale.x;
        SpawnSheep(5);
        
        //PlanSpawnFence(fenceLevel , transform.position.x , transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bluePrintPrefab, transform.position, transform.rotation);
        }
    }

    private void SpawnSheep(int amountOfSheep)
    {
        for (int i = 1; i <= amountOfSheep; i++)
        {

            Vector3 positionx = new Vector3(i, 0, 0);
            Vector3 positionz = new Vector3(0, 0, i);
            Vector3 position =  i % 2 != 0 ? positionx : positionz;
            position = i % 3 != 0 ? position : -position;
            GameObject sheepObj = Instantiate(sheepPrefab, transform.position + position, Quaternion.identity);
            sheepHerd.Add(sheepObj);

            
        }
        
    }
    private void PlanSpawnFence(int size, float xCenter, float zCenter)
    {
        // Local variable decleration
        float x = 0;
        float z = 0;
        float offsetCenter = ((size + 1) * fenceWidth) / 2f;
        Vector3 rotation = new Vector3(0, 0, 0);

        //Sqare Pattern Matrix
        int[,] sqrPattMatrix = new int[4, 4] { 
            { 1, 0, -1, 0 }, //Takes care of iterrations X-axis increment and decrement
            { 0, 1, 0, -1 }, //Takes care of iterrations Z-axis increment and decrement
            { 0, 1, -1, -1 }, //Takes care of iterrations X-axis corners
            { 0, 1, 1, -1 }   //Takes care of iterrations Z-axis corners
        };

        //Itteration of sides and corners
        for (int i = 0; i < 4; i++) //Goes throug each side of the sqare
        {
           
            //Corners
            x += sqrPattMatrix[2,i] * (fenceWidth / 2.5f);
            z += sqrPattMatrix[3,i] * (fenceWidth / 2.5f);
            //Rotate fences depending on even or odd side
            rotation.y = i % 2 == 0 ? 0.0f : 90.0f;
            //place a corner before staring side-loop, combine offset center, actual placemet of the fences and fence posotions.
            SpawnFence((x - offsetCenter) + xCenter, (z - offsetCenter) + zCenter, rotation);
            
            
            //Sides
            for (int y = 0; y < size; y++)
            {
                x += sqrPattMatrix[0, i] * fenceWidth;
                z += sqrPattMatrix[1, i] * fenceWidth;
                SpawnFence((x - offsetCenter) + xCenter, (z - offsetCenter) + zCenter, rotation);
                
            }
        }
    }

    private void SpawnFence(float x, float z, Vector3 rot)
    {
        Vector3 pos = new Vector3(x, 0.5f, z);
        GameObject fence = Instantiate(fencePrefab, pos, Quaternion.Euler(rot));
        fences.Add(fence);
    }
    public void UpgradeFence()
    {
        fenceLevel++;
        foreach (GameObject fence in fences)
        {
            
            Destroy(fence);
        }
        for (int i = fences.Count - 1; i > -1; i--)
        {
            
            fences.RemoveAt(i);
        }

        PlanSpawnFence(fenceLevel, transform.position.x, transform.position.z);
        
    }
}
