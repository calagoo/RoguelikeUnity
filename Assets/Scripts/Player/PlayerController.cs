using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20.0f;
    public float mouseSensitivity = 250.0f;
    private float horizontalInput;
    private float forwardInput;

    // Mouse rotation
    float xRotation = 0.0f;
    float yRotation = 0.0f;
    float mouseX;
    float mouseY;

    // E key Pickup
    private float grabHoldTime = 0.2f; // How long to hold to trigger physics grab
    private float eKeyTimer = 0f;
    // 

    public Rigidbody rb;
    public GameObject playerHead;
    public MainInventory mainInventory;
    public LaunchProjectile launchProjectile;
    public PhysicsGrabber physicsGrabber;
    public PickupHandler pickupHandler;
    public HotlistHandler hotlistHandler;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            mainInventory.OpenInventory();

        if (!mainInventory.inventoryParent.activeSelf) // If the inventory is not open
        {
            // Controls view and movement
            LookandMove();
            // --------------------------

            // Controls grabbing and releasing objects
            ItemPickup();
            // --------------------------

            // Controls Hotbar selection
            GetHotbarKey();
            // --------------------------

            // Controls shooting/throwing
            if (Input.GetButtonDown("Fire1"))
            {   
                launchProjectile.OnLaunchProjectile(1);
            }
            // --------------------------
        }
    }

    void LookandMove()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        forwardInput = Input.GetAxisRaw("Vertical");

        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        yRotation += mouseX;
        yRotation %= 360;

        // rotate the rigidbody up and down
        // rotate the player around the y axis
        rb.MoveRotation(Quaternion.Euler(0, yRotation, 0.0f));
        playerHead.transform.localRotation = Quaternion.Euler(xRotation, 0, 0.0f);

        Vector3 moveDirection = 
            (forwardInput * transform.forward + horizontalInput * transform.right).normalized;

        rb.MovePosition(transform.position + speed * Time.deltaTime * moveDirection);
    }

    void ItemPickup()
    {
        if (Input.GetKey(KeyCode.F))
        {
            eKeyTimer += Time.deltaTime;
            if (eKeyTimer > grabHoldTime)
            {
                physicsGrabber.TryGrab();
            }
        } 
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (eKeyTimer < grabHoldTime)
            {
                pickupHandler.PickUpItem(physicsGrabber.grabRange);
            } else
            {
                physicsGrabber.ReleaseGrab();
            }
            eKeyTimer = 0;
        }
        physicsGrabber.UpdateIndicator();
    }

    void GetHotbarKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Select hotbar slot 1
            hotlistHandler.SelectHotBar(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Select hotbar slot 2
            hotlistHandler.SelectHotBar(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Select hotbar slot 3
            hotlistHandler.SelectHotBar(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Select hotbar slot 4
            hotlistHandler.SelectHotBar(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Select hotbar slot 5
            hotlistHandler.SelectHotBar(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Select hotbar slot 6
            hotlistHandler.SelectHotBar(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Select hotbar slot 7
            hotlistHandler.SelectHotBar(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Select hotbar slot 8
            hotlistHandler.SelectHotBar(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Select hotbar slot 9
            hotlistHandler.SelectHotBar(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Select hotbar slot 0
            hotlistHandler.SelectHotBar(9);
        }
    }
}
