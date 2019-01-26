using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathMeter : MonoBehaviour
{
    public float SecondsOfBreath;
    private float maxBreath;
    private bool safeAtHome = false;

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
                SecondsOfBreath += Time.deltaTime * 2;
            }
        }

        if (SecondsOfBreath > maxBreath)
            SecondsOfBreath = maxBreath;
    }

    private void OnTriggerEnter2d(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Oasis")
        {
            safeAtHome = true;
        }
    }

    private void OnTriggerExit2d(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Oasis")
        {
            safeAtHome = false;
        }
    }
}