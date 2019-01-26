using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public static TopDownMovement instance;
    public Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    public BoxCollider2D bc2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    // public LayerMask blockingLayer;
    public float speed = 1;             //Floating point variable to store the player's movement speed.

    public bool busyHandlingInput = false;

    public enum PlayerState { Walking, Standing, Flinching, Attacking, }
    public enum Facing { North, South, East, West };
    public Facing facing;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    protected virtual void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        bc2D = gameObject.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    public virtual void FixedUpdate()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        //if (!this.GetComponent<PlayerStatus>().IsDead)
        //{
        HandleMovementInput();
        //}
    }

    public void HandleMovementInput()
    {
        Vector2 velocity = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            velocity.Set(velocity.x, speed);
            facing = Facing.North;
        }
        if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            velocity.Set(velocity.x, -(speed));
            facing = Facing.South;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            velocity.Set(-(speed), velocity.y);
            facing = Facing.West;
        }
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.Set(speed, velocity.y);
            facing = Facing.East;
        }

        rb2D.velocity = velocity;

        HandleMovementAnimations();
    }

    public void HandleMovementAnimations()
    {
        if (!rb2D.velocity.y.Equals(0.0f) || !rb2D.velocity.x.Equals(0.0f))
        {
            animator.SetFloat("speed", 1);
        }
        else if (rb2D.velocity.y.Equals(0.0f) && rb2D.velocity.x.Equals(0.0f))
        {
            animator.SetFloat("speed", 0);
        }

        //Call base triggers handler
        SetTriggers();
    }

    public virtual void SetTriggers()
    {
        animator.SetFloat("YSpeed", rb2D.velocity.y);
        animator.SetFloat("XSpeed", rb2D.velocity.x);

        if (rb2D.velocity.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb2D.velocity.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

}
