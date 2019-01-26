using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 velocity;

    public float smoothTimeX;
    public float smoothTimeY;
    public float smoothTimeZ;

    public bool isZoomedOut;

    public GameObject player;
    public Vector3 ZoomOutPosition;

    void Start()
    {
        if (ZoomOutPosition == null)
            ZoomOutPosition = new Vector3(0, 0, 0);
    }

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
        float posZ = transform.position.z;

        if (isZoomedOut)
        {
            posX = Mathf.SmoothDamp(transform.position.x, ZoomOutPosition.x, ref velocity.x, smoothTimeX*5);
            posY = Mathf.SmoothDamp(transform.position.y, ZoomOutPosition.y, ref velocity.y, smoothTimeY*5);
            posZ = Mathf.SmoothDamp(transform.position.z, ZoomOutPosition.z, ref velocity.z, smoothTimeZ*5);
        }

        transform.position = new Vector3(posX, posY, posZ);
    }
}
