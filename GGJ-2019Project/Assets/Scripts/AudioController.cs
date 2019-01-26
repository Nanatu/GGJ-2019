using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource atHome;
    public AudioSource awayHome;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Oasis")
        {
            if (awayHome.isPlaying)
            {
                awayHome.mute = true;
            }
            atHome.mute = false;
            //atHome.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Oasis")
        {
            if (atHome.isPlaying)
            {
                atHome.mute = true;
            }
            awayHome.mute = false;
            //atHome.Play();
        }
    }
}
