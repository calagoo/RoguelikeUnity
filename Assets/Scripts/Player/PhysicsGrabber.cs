using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class PhysicsGrabber : MonoBehaviour
{
    public GameObject player;
    public GameObject grabberIndicator;

    public readonly float grabRange = 3f;
    private float strengthForce;

    public bool isGrabbing = false;
    private GameObject currentObject;
    private float currentDistance;

    void Update()
    {
        strengthForce = PlayerStats.Instance.strength * 250f;
    }

    public void TryGrab()
    {
        if (isGrabbing) return;

        LayerMask layerMask = LayerMask.GetMask("Player");
        int mask = ~layerMask;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, grabRange, mask))
        {
            if (hit.collider.gameObject.layer == 7) // Item layer
            {
                isGrabbing = true;
                currentObject = hit.collider.gameObject;
                currentDistance = hit.distance;
            }
        }
    }

    public void ReleaseGrab()
    {
        isGrabbing = false;
    }

    public void UpdateIndicator()
    {
        if (grabberIndicator == null) return;

        if (isGrabbing)
        {
            grabberIndicator.SetActive(true);
            return;
        }
        LayerMask layerMask = LayerMask.GetMask("Player");
        int mask = ~layerMask;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, grabRange, mask))
        {
            if (hit.collider.gameObject.layer == 7)
            {
                grabberIndicator.SetActive(true);
                return;
            }
        }

        grabberIndicator.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isGrabbing && currentObject != null)
        {
            Vector3 targetPosition = player.transform.position + player.transform.forward * currentDistance + player.transform.up * 0.5f;
            Vector3 directionToTarget = targetPosition - currentObject.transform.position;
            float distance = directionToTarget.magnitude;

            if (distance > grabRange)
            {
                isGrabbing = false;
                return;
            }

            Rigidbody rb = currentObject.GetComponent<Rigidbody>();
            float damping = 30f;
            Vector3 force = distance / grabRange * strengthForce * directionToTarget.normalized - rb.linearVelocity * damping;
            rb.AddForce(force, ForceMode.Force);
        }
    }
}
