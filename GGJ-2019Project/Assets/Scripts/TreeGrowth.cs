using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{

    public float growthRate = 0.1f;
    private GameObject growthRing;

    public float decayTime = 2f;
    private float curTime = 0f;
    
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
        float scaleX = growthRing.transform.localScale.x + growthRate;
        float scaleY = growthRing.transform.localScale.y + growthRate;
        
        growthRing.transform.localScale = new Vector3(scaleX, scaleY);
        if (curTime <= decayTime)
        {
            curTime += Time.deltaTime;
        }
        else if (growthRate > 0.0f)
        {
            curTime = 0;
            growthRate -= 0.1f;
            if (growthRate < 0.0f)
            {
                growthRate = 0;
            }
        }

    }
}
