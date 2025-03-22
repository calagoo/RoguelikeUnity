using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float launchForce = 1000;
    public float destroyTime = 2;
    public PlayerMana playerMana;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnLaunchProjectile(float mana)
    {
        // Reduce mana
        if (playerMana != null && playerMana.Mana >= mana)
        {
            mana = projectilePrefab.GetComponent<ProjectileTestController>().mana;
            // projectilePrefab.mana = mana;
            playerMana.TakeDamage(mana);
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, launchForce));
            Destroy(projectile, destroyTime);
        }

    }
}
