using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public IGameAsset sourceAsset; // Assigned at projectile spawn
    private ProjectileData projectileData;
    private ExplosiveData explosiveData;

    private bool hasExploded = false;

    void Start()
    {
        projectileData = sourceAsset?.GetProjectileData();
        explosiveData = projectileData?.explosiveData;

        if (explosiveData != null && explosiveData.fuseType == ExplosiveData.FuseType.Timer)
        {
            Invoke(nameof(Explode), explosiveData.duration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            float damage = CalculateDamage(collision);
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
        }

        if (explosiveData != null)
        {
            if (explosiveData.fuseType == ExplosiveData.FuseType.Impact)
            {
                Explode();
            }
            else if (explosiveData.fuseType == ExplosiveData.FuseType.Proximity)
            {
                // Optional: check distance to target and explode
            }

            // For other fuse types, do nothing here — explosion will be triggered elsewhere
            return;
        }

        // No explosive behavior = destroy on impact
        Destroy(gameObject);
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

    float CalculateDamage(Collision collision)
    {
        if (projectileData == null) return 0f;

        float baseDamage = projectileData.damage;

        if (projectileData.isCorporeal)
        {
            float impactVelocity = collision.relativeVelocity.magnitude;
            return baseDamage * (impactVelocity / 20f);
        }

        return baseDamage;
    }
}
