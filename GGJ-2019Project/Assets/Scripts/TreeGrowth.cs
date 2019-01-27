using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{

    public float growthRate = 0.1f;
    private GameObject growthRing;

    public float decayTime = 2f;
    private float curTime = 0f;

    private float growth = 0.0f;
    public float maxGrowth = 1.5f;
    
    public float sproutStage = 0.2f;
    public float seedlingStage = 0.4f;
    public float tweenStage = 0.6f;
    public float adultStage = 0.8f;
    public float ancientStage = 1f;

    public Sprite sprout;
    public Sprite seedling;
    public Sprite tween;
    public Sprite adult;
    public Sprite ancient;

    private bool madeSeed = false;

    private ResourceManager resourceManager;
    
    // Start is called before the first frame update
    void Start()
    {
        //find growth ring
        growthRing = this.gameObject.transform.Find("Growth").gameObject;
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        
        resourceManager.SpawnResources(gameObject.transform.position);


    }

    // Update is called once per frame
    void Update()
    {

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

        if (growthRate > 0.0f)
        {
            GrowTree();
        }

        if (!madeSeed && growth > adultStage)
        {
            SpawnSeed();
        }
        

    }

    private void GrowTree()
    {

        if (growth < maxGrowth)
        {
            growth += growthRate;

            growthRing.transform.localScale = new Vector3(growth, growth);

            AgeTree();
        }
    }

    private void AgeTree()
    {
        if (growth >= seedlingStage && growth < tweenStage)
        {
            //change sprite/animate to seedling
            gameObject.GetComponent<SpriteRenderer>().sprite = seedling;
        }else if (growth >= tweenStage && growth < adultStage)
        {
            //change sprite/animate to tween
            gameObject.GetComponent<SpriteRenderer>().sprite = tween;
        }else if (growth >= adultStage && growth < ancientStage)
        {
            //change sprite/animate to adult
            gameObject.GetComponent<SpriteRenderer>().sprite = adult;
        }else if (growth >= ancientStage)
        {
            //change sprite/animate to ancient
            gameObject.GetComponent<SpriteRenderer>().sprite = ancient;
        }
    }

    //Add some resources and increase the growth rate
    public void AddResource(float resourceValue)
    {
        growthRate += resourceValue;
    }

    //Quickly add a Resource
    public void AddResource()
    {
        AddResource(0.1f);
    }

    private void SpawnSeed()
    {
        GameObject seed = resourceManager.seed;
        Instantiate(seed, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, gameObject.transform);
        
        madeSeed = true;
    }
}
