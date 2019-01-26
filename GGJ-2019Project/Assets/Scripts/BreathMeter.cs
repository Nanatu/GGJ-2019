﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathMeter : MonoBehaviour
{
    public float SecondsOfBreath;
    public float SecondsToPlantSeed;
    public float SecondsToRespawn;
    public bool isDead = false;
    public Vector2 RespawnPosition;
    private TopDownMovement topDownMovement;
    private float maxBreath;
    private float maxRespawnTimer;
    private float maxSeedPlantTimer;
    private bool safeAtHome = true;
    
    void Start()
    {
        if (RespawnPosition == null)
            RespawnPosition = new Vector2(0f, 0f);
        maxBreath = SecondsOfBreath;
        maxRespawnTimer = SecondsToRespawn;
        topDownMovement = GetComponent<TopDownMovement>();
    }

    void Update()
    {
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