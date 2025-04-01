using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    [HideInInspector]
    public float visionRange;
    [HideInInspector]
    public float visionAngle;
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public float reactionTime;
    [HideInInspector]
    public float rotationSpeed;
    public Skills skills;
    public EnemyStats stats;
    Dictionary<Skills.PassiveSkills, int> passiveSkills;
    public NavMeshAgent agent;
    protected Transform target;
    private Rigidbody rb;
    private Vector3 top;
    protected virtual void Start()
    {
        GenerateSkills();
        SkillsToStats(passiveSkills);
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody>();
        // Top using collider
        top = transform.position + Vector3.up * GetComponent<Collider>().bounds.extents.y;
    }

    protected virtual void Update()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < visionRange)
        {
            // Check if player is within vision angle
            Vector3 dirToPlayer = (target.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleToPlayer < visionAngle / 2)
            {
                // Player is within vision angle, move towards them
                MoveTowards(target.position);
            }
        }
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

    // Generate passive skills and use to deteremine stats
    void GenerateSkills()
    {
        passiveSkills = skills.GeneratePassiveSkills();
    }

    void SkillsToStats(Dictionary<Skills.PassiveSkills, int> passiveSkills)
    {
        // Default Skill Value == 4
        // View Distance from perception (Default == 50f) (0-100f)
        // View Angle from perception (Default == 45f) (0-180f)
        // Reaction time from reflex (Default == 0.5f)
        // Likelihood of retreating from willpower (Default == 0.5f (50% chance of retreating when low health))
        // Accuracy from precision (Default == 0.5f (50% chance of hitting))
        // Likelihood of resisting fear from resolve (Default == 0.5f (50% chance of resisting fear))

        // Move Speed is based on dexterity and strength (Default == 8f)

        visionRange = 50f + (passiveSkills[Skills.PassiveSkills.Perception] - 4) * 5f; // 0-100f
        visionAngle = 45f + (passiveSkills[Skills.PassiveSkills.Perception] - 4) * 1.5f; // 0-180f
        reactionTime = 0.5f - (passiveSkills[Skills.PassiveSkills.Reflex] - 4) * 0.05f; // 0-1f
        moveSpeed = Math.Clamp(8f + (stats.dexterity - 4) * 0.5f, 2, 20); // 0-10f

        
    }
}
