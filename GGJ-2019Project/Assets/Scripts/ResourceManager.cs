using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    
    public List<GameObject> resources;
    public GameObject seed;
    public GameObject tree;

    private GameObject carrier;
    
    
    private List<GameObject> spawnedResources;

    public int startingResources = 30;
    
    //set custom range for random position
    public float MinX = 0;
    public float MaxX = 10;
    public float MinY = 0;
    public float MaxY = 10;
 
    //for 3d you have z position
    public float MinZ = 0;
    public float MaxZ = 10;

    public float xRange = 10;
    public float yRange = 10;
    
    
    // Start is called before the first frame update
    void Start()
    {
        spawnedResources = new List<GameObject>();
        carrier = GameObject.FindWithTag("Player").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GenerateRandomLocation3D()
    {
        float x = Random.Range(MinX,MaxX);
        float y = Random.Range(MinY,MaxY);
        float z = Random.Range(MinZ,MaxZ);
        
        return new Vector3(x,y,z);
    }
    
    private Vector3 GenerateRandomLocation2D()
    {
        float x = Random.Range(MinX,MaxX);
        float y = Random.Range(MinY,MaxY);
        
        return new Vector3(x,y,0);
    }
    
    private  Vector3 GenerateRandomLocation2D(Vector3 spawnLocation)
    {
        float x = Random.Range(spawnLocation.x - xRange, spawnLocation.x + xRange);
        float y = Random.Range(spawnLocation.y - yRange ,spawnLocation.y + yRange);
        
        return new Vector3(x,y,0);
    }

    public void SpawnResources(Transform spawnLocation)
    {
        for (int i = 0; i < startingResources; i++)
        {
            int randomResource = Random.Range(0, resources.Count -1);
            Vector3 randomLocation = GenerateRandomLocation2D(spawnLocation.position);
            GameObject newResource = Instantiate(resources[randomResource], randomLocation, Quaternion.identity);
            newResource.transform.parent = spawnLocation;
            newResource.GetComponent<Orbit>().target = carrier.transform;
            spawnedResources.Add(newResource);
        }
        
    }
}