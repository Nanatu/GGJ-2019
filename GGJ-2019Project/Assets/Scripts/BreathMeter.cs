using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathMeter : MonoBehaviour
{
    public float SecondsOfBreath;
    public float SecondsToRespawn;
    public Vector2 RespawnPosition;
    
    private TopDownMovement topDownMovement;
    private float maxBreath;
    private float maxRespawnTimer;
    private bool safeAtHome = true;
    private bool isDead = false;

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
        if (isDead)
        {
            if (SecondsToRespawn > 0f)
                SecondsToRespawn -= Time.deltaTime;
            else
            {
                isDead = false;
                this.transform.position = RespawnPosition;
                SecondsToRespawn = maxRespawnTimer;
                SecondsOfBreath = maxBreath;
            }
        }
        else
        {
            if (!safeAtHome)
                SecondsOfBreath -= Time.deltaTime;
            else
            {
                if (SecondsOfBreath < maxBreath)
                {
                    SecondsOfBreath += Time.deltaTime * SecondsOfBreath;
                }
            }

            if (SecondsOfBreath > maxBreath)
                SecondsOfBreath = maxBreath;
            else if (SecondsOfBreath < 0f)
            {
                SecondsOfBreath = 0f;
                isDead = true;
            }
        }

        topDownMovement.enabled = !isDead;
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