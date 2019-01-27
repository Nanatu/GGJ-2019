using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScreenDriver : MonoBehaviour
{
    

    public GameObject wasd;
    public GameObject wasdImage;
    public GameObject e;
    public GameObject eImage;
    public GameObject z;
    public GameObject zImage;

    public int state = 0;

    /* enum States
    {
        wasdStuff,
        eStuff,
        zStuff
    } */

    void Start()
    {
        /* States myState;
        myState = States.wasdStuff; */
    }

    void Update()
    {
        if (state == 0)
        {
            wasd.SetActive(true);
            wasdImage.SetActive(true);
            e.SetActive(false);
            eImage.SetActive(false);
            z.SetActive(false);
            zImage.SetActive(false);

           if (Input.GetKeyUp("space"))
            {
                state = 1;
            }

        }

       else if (state == 1)
        {
            wasd.SetActive(false);
            wasdImage.SetActive(false);
            e.SetActive(true);
            eImage.SetActive(true);
            z.SetActive(false);
            zImage.SetActive(false);

            if (Input.GetKeyUp("space"))
            {
                state = 2;
            }
        }

       else if (state == 2)
        {

            wasd.SetActive(false);
            wasdImage.SetActive(false);
            e.SetActive(false);
            eImage.SetActive(false);
            z.SetActive(true);
            zImage.SetActive(true);

            if (Input.GetKeyUp("space"))
            {
                 SceneManager.LoadScene("StartScreen");
            }
        }
    }
}
