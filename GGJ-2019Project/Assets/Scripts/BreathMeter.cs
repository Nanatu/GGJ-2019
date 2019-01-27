using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathMeter : MonoBehaviour
{
    public float SecondsOfBreath;
    public float SecondsToPlantSeed;
    public float SecondsToRespawn;
    public bool isDead = false;
    public Vector2 RespawnPosition;
    public GameObject vignette;
    private TopDownMovement topDownMovement;
    private float maxBreath;
    private float maxRespawnTimer;
    public float maxSeedPlantTimer;
    public bool safeAtHome = true;

    public float visionSpeed = 0.1f;
    
    public bool onSafeAtHome
    {
        get => safeAtHome;
        set => safeAtHome = value;
    }

    void Start()
    {
        if (RespawnPosition == null)
            RespawnPosition = new Vector2(0f, 0f);
        maxBreath = SecondsOfBreath;
        maxRespawnTimer = SecondsToRespawn;
        maxSeedPlantTimer = SecondsToPlantSeed;
        topDownMovement = GetComponent<TopDownMovement>();
        vignette = GameObject.Find("Vignette");
    }

    void Update()
    {
        

        if (isDead && SecondsToRespawn > 0f)
        {
            SecondsToRespawn -= Time.deltaTime;
            ShrinkVision();
        }
        else
        {
            if (!safeAtHome)
            {
                if (topDownMovement.IsCarryingSeed)
                {
                    SecondsToPlantSeed -= Time.deltaTime;
                }
                else
                {
                    SecondsOfBreath -= Time.deltaTime;
                    ShrinkVision();
                }
            }
            else
            {
                if (SecondsOfBreath < maxBreath)
                {
                    SecondsOfBreath += Time.deltaTime * SecondsOfBreath;
                    
                }   
                IncreaseVision();
            }
        }

        if (SecondsOfBreath > maxBreath)
        {
            SecondsOfBreath = maxBreath;
        }
        if (SecondsOfBreath < 0f)
        {
            SecondsOfBreath = 0f;
            isDead = true;
        }
        if (SecondsToPlantSeed < 0f)
        {
            topDownMovement.PlantTree(transform.position);
        }
        if (SecondsToRespawn < 0f)
        {
            isDead = false;
            this.transform.position = RespawnPosition;
            SecondsToRespawn = maxRespawnTimer;
            SecondsOfBreath = maxBreath;
            FullVision();
        }
    }

    private void IncreaseVision()
    {
        Vector3 scale = vignette.transform.localScale;
        if (scale.x < 24 && scale.y < 24)
        {


            vignette.transform.localScale += new Vector3(visionSpeed, visionSpeed, 0);
        }

    }

    private void ShrinkVision()
    {
        Vector3 scale = vignette.transform.localScale;

        if (scale.x > 1.5f && scale.y > 1.5f)
        {

            vignette.transform.localScale -= new Vector3(visionSpeed, visionSpeed, 0);
        }
    }

    private void FullVision()
    {
        vignette.transform.localScale = new Vector3(24, 24, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Oasis" || other.tag == "Tree" || other.tag == "Seed")
            safeAtHome = true;
        else
            safeAtHome = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Oasis")
        {
            safeAtHome = false;
        }
    }
}