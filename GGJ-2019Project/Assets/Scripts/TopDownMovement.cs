using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public enum PlayerState { Walking, Standing, Flinching, Attacking, }
    public enum Facing { North, South, East, West };

    public static TopDownMovement instance;
    public BreathMeter breathMeter;
    public CameraController cameraController;
    public Facing facing;
    public Rigidbody2D rb2D;
    public BoxCollider2D bc2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public InventoryManager inventory;

    private float speed = 1;
    public float NormalSpeed = 1;
    public float CarryingSpeed = 2;
    public bool IsCarryingSeed = false;
    public float CameraZoomSpeed = 2;
    public bool busyHandlingInput = false;
    public int numOfResources = 0;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    protected virtual void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        bc2D = gameObject.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        breathMeter = GetComponent<BreathMeter>();
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();

        inventory = gameObject.GetComponentInChildren<InventoryManager>();
    }

    void Update()
    {
        if (IsCarryingSeed)
            speed = CarryingSpeed;
        else
            speed = NormalSpeed;
    }

    public virtual void FixedUpdate()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        HandleMovementInput();
        HandleCameraInput();
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

        if (breathMeter.isDead)
            velocity.Set(0, 0);

        rb2D.velocity = velocity;

        HandleMovementAnimations();
    }

    public void HandleMovementAnimations()
    {
        if (!rb2D.velocity.y.Equals(0.0f) || !rb2D.velocity.x.Equals(0.0f))
        {
            animator.SetFloat("Speed", 1);
        }
        else if (rb2D.velocity.y.Equals(0.0f) && rb2D.velocity.x.Equals(0.0f))
        {
            animator.SetFloat("Speed", 0);
        }

        SetTriggers();
    }

    public void HandleCameraInput()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            cameraController.isZooming = true;
        }
    }

    public virtual void SetTriggers()
    {
        animator.SetFloat("YSpeed", rb2D.velocity.y);
        animator.SetFloat("XSpeed", rb2D.velocity.x);

        if (rb2D.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb2D.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("PickUp"))
        {
            GameObject otherGameObject = other.gameObject;
            //other.gameObject.SetActive(false);
            otherGameObject.GetComponent<BoxCollider2D>().enabled = false;
            otherGameObject.GetComponent<Orbit>().IsActive = true;
            otherGameObject.transform.parent = inventory.transform;
            inventory.inventoriedResources.Add(otherGameObject);
            numOfResources++;
        }

        if (other.gameObject.CompareTag("Tree"))
        {
            if (inventory.inventoriedResources.Count > 0)
            {
                GameObject otherGameObject = other.gameObject;
                foreach (var resource in inventory.inventoriedResources)
                {
                    otherGameObject.GetComponent<TreeGrowth>().AddResource();
                    Destroy(resource);
                }
                inventory.inventoriedResources.Clear();
            }
        }
    }
}
