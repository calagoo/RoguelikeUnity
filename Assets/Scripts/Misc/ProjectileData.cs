using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Misc/Projectile")]
public class ProjectileData : ScriptableObject
{
    public enum MainDamageType
    {
    Physical,
    Magical
    }
    public enum SubDamageType
    {
    None,
    Fire,
    Ice,
    Lightning,
    Poison,
    Arcane,
    Holy,
    Shadow,
    Nature
    }
    public float damage;
    public float duration;
    public float speed;
    public float AOE;
    public bool isCorporeal; // If true, damage is based off physics
    public MainDamageType mainDamageType;
    public SubDamageType subDamageType;
    public GameObject prefab;
    public ExplosiveData explosiveData;
}
