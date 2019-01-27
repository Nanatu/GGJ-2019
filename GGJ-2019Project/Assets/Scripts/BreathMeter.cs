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
    private float maxSeedPlantTimer;
    private bool safeAtHome = true;

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
        topDownMovement = GetComponent<TopDownMovement>();
        vignette = GameObject.Find("Vignette");
    }

    void Update()
    {
        var vignetteScaleX = Time.deltaTime;
        var vignetteScaleY = Time.deltaTime;

        if (isDead && SecondsToRespawn > 0f)
        {
            SecondsToRespawn -= Time.deltaTime;
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
                    if(vignette.transform.localScale.x > 1.1)
                        vignette.transform.localScale -= new Vector3(vignetteScaleX,0,0);
                    if(vignette.transform.localScale.y > 1.6)
                        vignette.transform.localScale -= new Vector3(0,vignetteScaleY,0);
                }
            }
            else
            {
                if (SecondsOfBreath < maxBreath)
                {
                    SecondsOfBreath += Time.deltaTime * SecondsOfBreath;
                    if(vignette.transform.localScale.x > 1.1)
                        vignette.transform.localScale += new Vector3(vignetteScaleX,0,0);
                    if(vignette.transform.localScale.y > 1.6)
                        vignette.transform.localScale += new Vector3(0,vignetteScaleY,0);
                }   
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
            topDownMovement.IsCarryingSeed = false;
            SecondsToPlantSeed = maxSeedPlantTimer;
        }
        if (SecondsToRespawn < 0f)
        {
            isDead = false;
            this.transform.position = RespawnPosition;
            SecondsToRespawn = maxRespawnTimer;
            SecondsOfBreath = maxBreath;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Oasis")
        {
            safeAtHome = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Oasis")
        {
            safeAtHome = false;
        }
    }
    
    
}