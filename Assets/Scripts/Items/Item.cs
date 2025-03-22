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
        Misc
    }

    public string itemName;
    public ItemCategory itemCategory;
    public int itemID;
    public string itemDescription;
    public int itemValue;
    public bool isConsumable;
    public int itemRarity;
    public int itemWeight;
    Rigidbody rb => GetComponent<Rigidbody>();
    // Start is called before the first frame update
    void Start()
    {
        rb.mass = itemWeight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
