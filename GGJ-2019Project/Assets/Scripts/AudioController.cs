using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource atHome;
    public AudioSource awayHome;
    public bool inOasis = true;
    public const float fullSoundTransitionTime = 3.0f;  // Time in seconds for a full switch of audio to occur.
    public float volumePerUpdate;
    
    // Start is called before the first frame update
    void Start()
    {
        atHome.Play();
        awayHome.Play();
        atHome.volume = 1.0f;
        awayHome.volume = 0.0f;
        volumePerUpdate = Time.fixedDeltaTime / fullSoundTransitionTime;
    }

    // FixedUpdate is called on a set period.
    void FixedUpdate()
    {
        if (inOasis)
        {
            atHome.volume += (volumePerUpdate);
            atHome.volume = Mathf.Min(atHome.volume, 1.0f);
        }
        else
        {
            atHome.volume -= (volumePerUpdate);
            atHome.volume = Mathf.Max(atHome.volume, 0.0f);
        }

        awayHome.volume = 1.0f - atHome.volume;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Oasis")
        {
            inOasis = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Oasis")
        {
            inOasis = false;
        }
    }
}
