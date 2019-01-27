using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 velocity;
    private float minZoomDistance;
    
    public int zoomDirection = 1;
    public float smoothTimeX;
    public float smoothTimeY;
    public float zoomSpeed;
    public float maxZoomDistance;
    public bool isZooming;

    public GameObject player;
    public Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        minZoomDistance = mainCamera.orthographicSize;
    }

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (isZooming)
        {
            mainCamera.orthographicSize += zoomSpeed * zoomDirection;
            if(mainCamera.orthographicSize >= maxZoomDistance || mainCamera.orthographicSize <= minZoomDistance)
            {
                isZooming = false;
                zoomDirection = zoomDirection * -1;
            }
        }
    }
}
