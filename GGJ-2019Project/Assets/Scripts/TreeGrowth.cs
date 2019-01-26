using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{

    public float growthRate = 0.4f;
    private GameObject growthRing;
    
    // Start is called before the first frame update
    void Start()
    {
        //find growth ring
        growthRing = this.gameObject.transform.Find("Growth").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //increase growthRing based on growth rate
        
    }
}
