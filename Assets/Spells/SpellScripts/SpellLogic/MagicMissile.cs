using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicMissile", menuName = "Spells/Logic/Magic Missile")]
public class MagicMissile : SpellLogic
{
    public override void Execute(GameObject caster, Vector3 target, SpellData data)
    {

        // Fixing the missile rotation
        Vector3 dir = (target - caster.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0);
        rotation *= Quaternion.AngleAxis(-90, Vector3.forward); // Local X axis
        //

        GameObject missile = Instantiate(data.projectileData.projectilePrefab, caster.transform.position, rotation);
        missile.GetComponent<ProjectileHandler>().sourceAsset = data;

        missile.GetComponent<Rigidbody>().velocity = dir * data.projectileData.speed;

        GameObject.Destroy(missile, data.projectileData.duration);
    }
}
