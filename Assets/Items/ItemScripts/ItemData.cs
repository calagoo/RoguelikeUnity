using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject, IGameAsset
{
    public enum ItemCategory
    {
        Weapon,
        Armor,
        Consumable,
        QuestItem,
        Ammo,
        Resource,
        Utility,
        Misc,
        Trash
    }

    public enum ConsumableType
    {
        None,
        Edible,
        Usable,
        Both
    }

    [Header("Identification")]
    public int id = -1; // Default: unassigned

    [Header("Basic Info")]
    public string itemName;
    [TextArea] public string description;
    public int baseValue;
    public int rarity;
    public float weight;

    [Header("Visuals")]
    public Sprite icon;
    public GameObject prefab;

    [Header("Classification")]
    public ItemCategory category;
    public ConsumableType consumableType;

    [Header("Projectile (optional)")]
    public ThrowLogic throwLogic; // Null if this item isn't throwable
    public ProjectileData projectileData; // Null if this item isn't a projectile

    // ---- IGameAsset Implementation ----
    public int ID => id;
    public string Name => itemName;
    public Sprite Icon => icon;
    public ProjectileData GetProjectileData() => projectileData;
    public GameObject GetPrefab() => prefab;
}
