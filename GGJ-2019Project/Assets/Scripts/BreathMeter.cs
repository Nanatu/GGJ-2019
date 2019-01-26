using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathMeter : MonoBehaviour
{
    public float SecondsOfBreath;
    private float maxBreath;
    private bool safeAtHome = true;

    void Start()
    {
        maxBreath = SecondsOfBreath;
    }

    void Update()
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