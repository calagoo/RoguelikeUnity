using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    private Dictionary<string, ItemData> nameLookup;

    public void Init()
    {
        nameLookup = new();
        foreach (var item in items)
            nameLookup[item.itemName] = item;
    }

    public ItemData GetByName(string name)
    {
        if (nameLookup == null) Init();
        return nameLookup.TryGetValue(name, out var data) ? data : null;
    }

    public ItemData GetRandomItem()
    {
        Debug.Log("Getting random item");
        int randomIndex = Random.Range(0, items.Count);
        Debug.Log("Random index: " + randomIndex);
        return items[randomIndex];
    }

    public ItemData GetByID(int id)
    {
        return items[id];
    }
}