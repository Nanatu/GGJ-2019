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

    private GameObject seed;

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
        HandleActionInput();
    }

    public void HandleMovementInput()
    {
        Vector2 velocity = new Vector2(0, 0);

        if ((Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) ||
            (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
        {
            velocity.Set(velocity.x, speed);
            facing = Facing.North;
        }
        if ((Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) ||
            (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)))
        {
            velocity.Set(velocity.x, -(speed));
            facing = Facing.South;
        }

        if ((Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) ||
            (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
        {
            velocity.Set(-(speed), velocity.y);
            facing = Facing.West;
        }
        if ((Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) ||
            (Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.A))
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

    public void HandleActionInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (IsCarryingSeed)
            {
                Vector3 currentPosition = gameObject.transform.position;

                if (!GetComponent<BreathMeter>().onSafeAtHome)
                {
                    PlantTree(currentPosition);
                }
            }


            //pick up seed
            if (seed && !IsCarryingSeed)
            {
                seed.transform.parent = gameObject.transform;
                seed.GetComponent<PolygonCollider2D>().enabled = false;

                seed.transform.localPosition = Vector3.zero;
                IsCarryingSeed = true;
            }
        }
    }

    public void PlantTree(Vector3 position)
    {
        if (IsCarryingSeed)
        {
            IsCarryingSeed = false;
            GameObject tree = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().tree;
            Instantiate(tree, position, Quaternion.identity);
            Destroy(seed);
            seed = null;
            breathMeter.SecondsToPlantSeed = breathMeter.maxSeedPlantTimer;
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
        if (other.gameObject.CompareTag("PickUp"))
        {
            GameObject otherGameObject = other.gameObject;

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

        if (other.gameObject.CompareTag("Seed"))
        {
            GameObject otherGameObject = other.gameObject;
            seed = otherGameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Seed"))
        {
            GameObject otherGameObject = other.gameObject;
            seed = null;
        }
    }
}
