using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAI : MonoBehaviour
{
    public float visionRange;
    public float moveSpeed;
    public float rotationSpeed;
    protected Transform target;
    private Rigidbody rb;
    private Vector3 top;
    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody>();
        // Top using collider
        top = transform.position + Vector3.up * GetComponent<Collider>().bounds.extents.y;
    }

    protected virtual void Update()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < visionRange)
            MoveTowards(target.position);
    }

    protected void MoveTowards(Vector3 position)
    {
        Vector3 dir = (position - transform.position).normalized;

        bool standing = CheckIfStanding();

        if (!standing)
        {
            // Get top of collider in current rotation
            Collider col = GetComponent<Collider>();
            Vector3 topWorld = col.bounds.center + transform.up * col.bounds.extents.y;

            if (Vector3.Dot(transform.up, Vector3.up) > 0.98f)
                return;

            Vector3 recoveryForce = -Physics.gravity * rb.mass * 0.125f;
            rb.AddForceAtPosition(recoveryForce, topWorld, ForceMode.Force);
            return;
        }

        // Movement
        Vector3 move = moveSpeed * Time.deltaTime * dir;
        rb.MovePosition(rb.position + move);

        // Rotation toward target
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        // Draw Ray
        Debug.DrawRay(transform.position, dir * 10, Color.red);
        Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(newRotation);

        // Only constrain rotation (keep Y free)
        LockStanding(newRotation);
    }


    public bool CheckIfStanding()
    {
        Vector3 rot = transform.eulerAngles;

        // Normalize angles (Unity returns 0–360)
        float x = NormalizeAngle(rot.x);
        float z = NormalizeAngle(rot.z);

        // Standing if rotation is roughly upright
        if (Mathf.Abs(x) > 20f || Mathf.Abs(z) > 20f)
            return false;

        return true;
    }

    private float NormalizeAngle(float angle)
    {
        // Convert 0–360 range to -180 to 180
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }

    void LockStanding(Quaternion quaternion)
    {
        // Preserve current Y rotation, zero out X and Z
        Quaternion newRotation = Quaternion.Euler(0, quaternion.eulerAngles.y, 0);
        rb.MoveRotation(newRotation);
    }

    void UnlockStanding()
    {
        // Allow rotation on all axes
        rb.constraints = RigidbodyConstraints.None;
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        UnlockStanding();
        moveSpeed = 0;
        yield return new WaitForSeconds(duration);
        moveSpeed = 2f;
    }
}
