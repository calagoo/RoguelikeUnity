using UnityEngine;
using UnityEngine.VFX;

public class ExplosiveHandler : MonoBehaviour
{
    public ExplosiveData explosiveData;

    private float fuseTimer;
    public float FusePercent => 1 - Mathf.Clamp01(fuseTimer / explosiveData.duration);
    public Material fuseMaterial;
    public VisualEffect fuseSpark;
    private bool isFuseLit = false;
    private bool hasExploded = false;

    public void Start()
    {
        if (explosiveData.fuseType == ExplosiveData.FuseType.Timer)
        {
            StartFuse();
        }   
    }

    void Update()
    {
        if (explosiveData.fuseType != ExplosiveData.FuseType.Timer) return;
        
        if (!isFuseLit) return;

        fuseTimer -= Time.deltaTime;

        // Update shader
        if (fuseMaterial != null)
        {
            fuseMaterial.SetFloat("_FusePercent", 1-FusePercent);
        }

        // Update visual effect
        if (fuseSpark != null)
        {
            fuseSpark.SetFloat("FusePercent", FusePercent);
        }

        if (fuseTimer <= 0f)
        {
            Detonate();
        }
    }

    public void StartFuse()
    {
        fuseTimer = explosiveData.duration;
        isFuseLit = true;

    }

    public void Impact()
    {
        if (explosiveData.fuseType == ExplosiveData.FuseType.Impact)
        {
            Detonate();
        }
    }

    public void Detonate()
    {
        isFuseLit = false;
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Debug.Log($"💥 Explosion triggered: {gameObject.name}");

        // Optional: visual effect
        // if (explosiveData.explosionVFX != null)
        // {
        //     Instantiate(explosiveData.explosionVFX, transform.position, Quaternion.identity);
        // }

        if (explosiveData.explosiveType == ExplosiveData.ExplosiveType.Physical)
        {
            // Area damage
            Collider[] hits = Physics.OverlapSphere(transform.position, explosiveData.radius);
            foreach (var hit in hits)
            {
                // Physical explosion: Concussive, damages through walls, damage fall off
                // Damage is based on distance from explosion
                // Damage = baseDamage * (1 - distance / radius*2)
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                float damage = explosiveData.damage * (1 - distance / (explosiveData.radius * 2));

                // Use IDamageable interface to apply damage
                IDamageable damageable = hit.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);

                // Apply knockback
                if (hit.TryGetComponent<Rigidbody>(out var rb))
                {
                    // Knockback: damage
                    float knockback = damage * damage;
                    rb.AddExplosionForce(knockback, transform.position, explosiveData.radius);
                    // Stun
                    if (hit.TryGetComponent<NPCAI>(out var npc))
                    {
                        npc.Stun(2f);
                    }
                }

            }
        }
        else if (explosiveData.explosiveType == ExplosiveData.ExplosiveType.Magical)
        {
            // Area effect
            Collider[] hits = Physics.OverlapSphere(transform.position, explosiveData.radius);
            foreach (var hit in hits)
            {
                // Regular explosion with magic damage type
            }
        }
        else if (explosiveData.explosiveType == ExplosiveData.ExplosiveType.Fragmentation)
        {
            // No damage fall off, but must be line of sight from explosion
            Collider[] hits = Physics.OverlapSphere(transform.position, explosiveData.radius);
            foreach (var hit in hits)
            {
                // Check if hit is in line of sight
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, hit.transform.position - transform.position, out hitInfo, explosiveData.radius))
                {
                    // Use IDamageable interface to apply damage
                    IDamageable damageable = hit.GetComponent<IDamageable>();
                    damageable?.TakeDamage(explosiveData.damage);
                }
            }
        }

        Destroy(gameObject);
    }


}