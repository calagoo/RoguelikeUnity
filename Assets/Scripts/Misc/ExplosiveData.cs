using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "Misc/Explosive")]
public class ExplosiveData : ScriptableObject
{
    public enum ExplosiveType
    {
        Physical,
        Magical,
        Fire,
        Fragmentation
    }
    public enum FuseType
    {
        Impact,
        Timer,
        Proximity
    }
    public float damage;
    public float radius;
    public float duration;
    public ExplosiveType explosiveType;
    public FuseType fuseType;
    public GameObject explosionPrefab;
}
