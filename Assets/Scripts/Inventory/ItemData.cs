using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
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
    public int id = -1; // Default: unassigned
    public string itemName;
    public string description;
    public int baseValue;
    public int rarity;
    public float weight;
    public Sprite icon;
    public GameObject prefab;

    public ItemCategory category;
    public ConsumableType consumableType;
}
