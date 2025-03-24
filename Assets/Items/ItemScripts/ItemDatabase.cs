using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [HideInInspector] public List<ItemData> items;
    [HideInInspector] public List<SpellData> spells;
    
    [System.NonSerialized] public List<IGameAsset> gameAssets;

    private void OnEnable()
    {
        gameAssets = new List<IGameAsset>();

        if (items != null) gameAssets.AddRange(items);
        if (spells != null) gameAssets.AddRange(spells);
    }

    public IGameAsset GetByID(int id)
    {
        return gameAssets?.FirstOrDefault(asset => asset.ID == id);
    }

    public ItemData GetItemByID(int id)
    {
        return GetByID(id) as ItemData;
    }

    public SpellData GetSpellByID(int id)
    {
        return GetByID(id) as SpellData;
    }

    public ItemData GetRandomItem()
    {
        var itemList = gameAssets.OfType<ItemData>().ToList();
        if (itemList.Count == 0) return null;

        int randomIndex = Random.Range(0, itemList.Count);
        return itemList[randomIndex];
    }
}
