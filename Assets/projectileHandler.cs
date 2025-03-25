using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public IGameAsset sourceAsset; // Assigned at projectile spawn
    private ProjectileData projectileData;
    private ExplosiveData explosiveData;
    public ExplosiveHandler explosiveHandler;

    void Start()
    {
        projectileData = sourceAsset?.GetProjectileData();
        explosiveData = projectileData?.explosiveData;

        if (explosiveData != null)
        {
            explosiveHandler.explosiveData = explosiveData;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float damage = CalculateDamage(collision);
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
        }

        explosiveHandler.Impact();
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
