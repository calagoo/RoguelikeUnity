using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // All main item categories:
    // Weapons, Armor, Consumables, Quest Items, Ammo
    // Resources, Utility, Misc

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

    public string itemName;
    public string itemDescription;
    public int itemValue;
    public int itemRarity;
    public int itemWeight;
    public GameObject prefabReference;
    public Sprite itemIcon;
    public ItemCategory itemCategory;
    public ConsumableType consumableType;

    Rigidbody rb => GetComponent<Rigidbody>();
    // Start is called before the first frame update
    void Awake()
    {
        rb.mass = itemWeight;
        if (itemCategory == ItemCategory.Trash)
        {
            itemValue = 0;
        }

        if (prefabReference == null)
        {
            prefabReference = gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
