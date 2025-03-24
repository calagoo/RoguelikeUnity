using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrownItem", menuName = "Items/Logic/ThrownItem")]
public class ThrownItem : ThrowLogic
{
    public override void Execute(GameObject caster, Vector3 target, ItemData data)
    {
        GameObject ThrownItem = Instantiate(data.projectileData.projectilePrefab, caster.transform.position, Quaternion.identity);
        ThrownItem.GetComponent<ProjectileHandler>().sourceAsset = data;
        ThrownItem.GetComponent<Rigidbody>().velocity = (target - caster.transform.position).normalized * data.projectileData.speed;
        GameObject.Destroy(ThrownItem, data.projectileData.duration);
    }
}
