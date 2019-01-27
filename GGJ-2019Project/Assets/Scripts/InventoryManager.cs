using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public List<GameObject> inventoriedResources;
    
    // Start is called before the first frame update
    void Start()
    {
        inventoriedResources = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
