using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spells/Spell")]
public class SpellData : ScriptableObject, IGameAsset
{
    [Header("Identification")]
    public int id = -1; // Default: unassigned

    [Header("Basic Info")]
    public string spellName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Mechanics")]
    public float manaCost;
    public float cooldown;

    public enum SpellTargetingMode
    {
        Self,
        Touch,
        Target,
        Directional
    }

    public SpellTargetingMode targetingMode;

    [Header("Logic & Projectile")]
    public SpellLogic spellLogic;
    public ProjectileData projectileData; // Null if this spell isn't a projectile

    // ---- IGameAsset Implementation ----
    public int ID => id;
    public string Name => spellName;
    public Sprite Icon => icon;
    public ProjectileData GetProjectileData() => projectileData;
    public GameObject GetPrefab() => null; // Optional: Replace with prefab if you want spell visuals
}
