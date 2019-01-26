using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector2 velocity;

    public float smoothTimeX;
    public float smoothTimeY;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x,ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y,ref velocity.y, smoothTimeY);
    
        transform.position = new Vector3(posX, posY, transform.position.z);

    }
}
    