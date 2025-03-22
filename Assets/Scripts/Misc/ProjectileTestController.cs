using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTestController : MonoBehaviour
{
    GameObject projectilePrefab;
    public float mana = 1;
    public float normalDamage = 10; // 20 damage at 10 m/s
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            float relativeVelocity = collision.relativeVelocity.magnitude;
            GameObject enemy = collision.gameObject;
            if (relativeVelocity < 3)
            {
                return;
            }
            normalDamage = normalDamage * (relativeVelocity / 10);
            Debug.Log("Hit " + enemy.name +  " for " + normalDamage + " damage");
            enemy.GetComponent<EnemyHealth>().TakeDamage(normalDamage);
        }
    }
}
