using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
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

    public MainInventory mainInventory;
    public LaunchProjectile launchProjectile;
    public PhysicsGrabber physicsGrabber;
    public PickupHandler pickupHandler;

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
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        yRotation += mouseX;
        yRotation = yRotation % 360;

        // rotate the rigidbody up and down
        // rotate the player around the y axis
        transform.localRotation = Quaternion.Euler(0, yRotation, 0.0f);

        rb.transform.localRotation = Quaternion.Euler(xRotation, 0, 0.0f);


        // Move the player forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        // Strafe the player
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);   
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
}
