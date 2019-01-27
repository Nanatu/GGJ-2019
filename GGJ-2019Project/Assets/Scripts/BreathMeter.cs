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
                }
            }
            else
            {
                if (SecondsOfBreath < maxBreath)
                {
                    SecondsOfBreath += Time.deltaTime * SecondsOfBreath;
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
            topDownMovement.PlantTree(transform.localPosition);
        }
        if (SecondsToRespawn < 0f)
        {
            isDead = false;
            this.transform.position = RespawnPosition;
            SecondsToRespawn = maxRespawnTimer;
            SecondsOfBreath = maxBreath;
        }
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