using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{

    public Transform target;

    private float percentRotation;

    public float speed =.02f;
    public float targetDistance = 1;
    public float mass = 1;
    public float springyness = 1;
    public float efficiency = .2f;

    public bool IsActive;


    private float springVelocity;
    private float curRadius;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var toTarget = (this.transform.position - target.transform.position);

        //figure out where in the orbit the two are so it starts orbiting in the right spot
        if (curRadius > .3f)
        {
            var angle = Mathf.Atan2(toTarget.y, toTarget.x);
            percentRotation = angle / (2 * Mathf.PI);
        }

        curRadius = toTarget.magnitude;
        //Debug.Log($"angle: {angle}");

        if(IsActive && target != null)
        {
            //loop percent
            percentRotation += speed * Time.deltaTime;
            if (percentRotation > 1)
            {
                percentRotation = 0;
            }

            //raidal (gravity)
            var offset = targetDistance - curRadius;
            var force = (offset * springyness);
            var accel = force / mass;
            springVelocity += accel * Time.deltaTime + (-Mathf.Sign(springVelocity) *  efficiency * Time.deltaTime);
            curRadius += springVelocity * Time.deltaTime ;

            var x = curRadius * Mathf.Cos(percentRotation * 2 * Mathf.PI) + target.transform.position.x;
            var y = curRadius * Mathf.Sin(percentRotation * 2 * Mathf.PI) + target.transform.position.y;
            this.transform.position = new Vector3(x, y, this.transform.position.z);
        }

    }
}
