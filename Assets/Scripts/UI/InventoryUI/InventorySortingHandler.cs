using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySortingHandler : MonoBehaviour
{
    public Sprite[] sortingButtonSprites;
    public GameObject[] sortingSprites;
    public int currentSortType = 0;
    InventoryHandler inventoryHandler;
    ItemDatabase itemDatabase;
    Dictionary<int, int> inventoryItemCounts = new Dictionary<int, int>();
    private class ItemAttributes
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Rarity { get; set; }
        public float Weight { get; set; }
        public int Value { get; set; }
        public int ID { get; set; }
    }


    public void Start()
    {
        inventoryHandler = GetComponent<InventoryHandler>();
        itemDatabase = inventoryHandler.itemDatabase;
    }

    public void SortInventoryItems(int sortType)
    {
        inventoryItemCounts = inventoryHandler.inventoryItemCounts;

        // Sort types:
        // 0 = Name (A-Z)
        // 1 = Name (Z-A)
        // 2 = Quantity (Low-High)
        // 3 = Quantity (High-Low)
        // 4 = Rarity (Low-High)
        // 5 = Rarity (High-Low)
        // 6 = Weight (Low-High)
        // 7 = Weight (High-Low)
        // 8 = Value (Low-High)
        // 9 = Value (High-Low)
        if (currentSortType == sortType)
        {
            currentSortType = sortType + 1;
        }
        else if (currentSortType == sortType + 1)
        {
            currentSortType = 0;
        }
        else
        {
            currentSortType = sortType;
        }

        // Sprites:
        // 0 = Ascending
        // 1 = Descending
        // 2 = Empty
        switch (currentSortType)
        {
            case 0: // Name (A-Z)
                SetSprites(0, true);
                inventoryItemCounts = SortByName(inventoryItemCounts, true);
                break;
            case 1: // Name (Z-A)
                SetSprites(0, false);
                inventoryItemCounts = SortByName(inventoryItemCounts, false);
                break;
            case 2: // Quantity (High-low)
                SetSprites(1, true);
                inventoryItemCounts = SortByQuantity(inventoryItemCounts, false);
                break;
            case 3: // Quantity (Low-High)
                SetSprites(1, false);
                inventoryItemCounts = SortByQuantity(inventoryItemCounts, true);
                break;
            case 4: // Rarity (High-low)
                SetSprites(2, true);
                inventoryItemCounts = SortByRarity(inventoryItemCounts, false);
                break;
            case 5: // Rarity (Low-High)
                SetSprites(2, false);
                inventoryItemCounts = SortByRarity(inventoryItemCounts, true);
                break;
            case 6: // Weight (High-low)
                SetSprites(3, true);
                inventoryItemCounts = SortByWeight(inventoryItemCounts, false);
                break;
            case 7: // Weight (Low-High)
                SetSprites(3, false);
                inventoryItemCounts = SortByWeight(inventoryItemCounts, true);
                break;
            case 8: // Value (High-low)
                SetSprites(4, true);
                inventoryItemCounts = SortByValue(inventoryItemCounts, false);
                break;
            case 9: // Value (Low-High)
                SetSprites(4, false);
                inventoryItemCounts = SortByValue(inventoryItemCounts, true);
                break;
            default:
                Debug.LogWarning("Invalid sort type selected: " + sortType);
                break;
        }
        inventoryHandler.inventoryItemCounts = inventoryItemCounts;
        inventoryHandler.RenderItems();
    }

    private void SetSprites(int buttonIndex, bool ascending)
    {
        // Sprites:
        // 0 = Ascending
        // 1 = Descending
        // 2 = Empty
        if (ascending)
        {
            sortingSprites[buttonIndex].GetComponent<UnityEngine.UI.Image>().sprite = sortingButtonSprites[0];
            sortingSprites[buttonIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            sortingSprites[buttonIndex].GetComponent<UnityEngine.UI.Image>().sprite = sortingButtonSprites[1];
            sortingSprites[buttonIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 1f);
        }

        for (int i = 0; i < sortingSprites.Length; i++)
        {
            if (i != buttonIndex)
            {
                sortingSprites[i].GetComponent<UnityEngine.UI.Image>().sprite = sortingButtonSprites[2];
                sortingSprites[i].GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    List<ItemAttributes> ConvertToAttributes(Dictionary<int,int> inventoryItemCounts)
    {
        List<ItemAttributes> attributes = new();

        foreach (KeyValuePair<int, int> item in inventoryItemCounts)
        {
            ItemAttributes attribute = new()
            {
                Name = itemDatabase.items[item.Key].itemName,
                Quantity = item.Value, // Quantity
                Rarity = itemDatabase.items[item.Key].rarity,
                Weight = itemDatabase.items[item.Key].weight,
                Value = itemDatabase.items[item.Key].baseValue,
                ID = item.Key
            };
            attributes.Add(attribute);
        }   

        return attributes;
    }

    Dictionary<int,int> ConvertToItemCounts(List<ItemAttributes> attributes)
    {
        Dictionary<int, int> inventoryItemCounts = new();

        foreach (ItemAttributes attribute in attributes)
        {
            inventoryItemCounts.Add(attribute.ID, attribute.Quantity);
        }

        return inventoryItemCounts;
    }

    public Dictionary<int,int> SortByName(Dictionary<int,int> inventoryItemCounts, bool ascending)
    {
        List<ItemAttributes> attributes = ConvertToAttributes(inventoryItemCounts);

        if (ascending)
        {
            attributes.Sort((x, y) => string.Compare(x.Name, y.Name));
        }
        else
        {
            attributes.Sort((x, y) => string.Compare(y.Name, x.Name));
        }

        inventoryItemCounts = ConvertToItemCounts(attributes);
        return inventoryItemCounts;
    }

    public Dictionary<int,int> SortByQuantity(Dictionary<int,int> inventoryItemCounts, bool ascending)
    {
        List<ItemAttributes> attributes = ConvertToAttributes(inventoryItemCounts);

        if (ascending)
        {
            attributes.Sort((x, y) => x.Quantity.CompareTo(y.Quantity));
        }
        else
        {
            attributes.Sort((x, y) => y.Quantity.CompareTo(x.Quantity));
        }

        inventoryItemCounts = ConvertToItemCounts(attributes);
        return inventoryItemCounts;
    }

    public Dictionary<int,int> SortByRarity(Dictionary<int,int> inventoryItemCounts, bool ascending)
    {
        List<ItemAttributes> attributes = ConvertToAttributes(inventoryItemCounts);

        if (ascending)
        {
            attributes.Sort((x, y) => x.Rarity.CompareTo(y.Rarity));
        }
        else
        {
            attributes.Sort((x, y) => y.Rarity.CompareTo(x.Rarity));
        }

        inventoryItemCounts = ConvertToItemCounts(attributes);
        return inventoryItemCounts;
    }

    public Dictionary<int,int> SortByWeight(Dictionary<int,int> inventoryItemCounts, bool ascending)
    {
        List<ItemAttributes> attributes = ConvertToAttributes(inventoryItemCounts);

        if (ascending)
        {
            attributes.Sort((x, y) => x.Weight.CompareTo(y.Weight));
        }
        else
        {
            attributes.Sort((x, y) => y.Weight.CompareTo(x.Weight));
        }

        inventoryItemCounts = ConvertToItemCounts(attributes);
        return inventoryItemCounts;
    }

    public Dictionary<int,int> SortByValue(Dictionary<int,int> inventoryItemCounts, bool ascending)
    {
        List<ItemAttributes> attributes = ConvertToAttributes(inventoryItemCounts);

        if (ascending)
        {
            attributes.Sort((x, y) => x.Value.CompareTo(y.Value));
        }
        else
        {
            attributes.Sort((x, y) => y.Value.CompareTo(x.Value));
        }

        inventoryItemCounts = ConvertToItemCounts(attributes);
        return inventoryItemCounts;
    }




    public void SortInventoryItems_IH()
    {
        inventoryItemCounts = inventoryHandler.inventoryItemCounts;

        // Sort types:
        // 0 = Name (A-Z)
        // 1 = Name (Z-A)
        // 2 = Quantity (Low-High)
        // 3 = Quantity (High-Low)
        // 4 = Rarity (Low-High)
        // 5 = Rarity (High-Low)
        // 6 = Weight (Low-High)
        // 7 = Weight (High-Low)
        // 8 = Value (Low-High)
        // 9 = Value (High-Low)

        // Sprites:
        // 0 = Ascending
        // 1 = Descending
        // 2 = Empty

        SetSprites(0, true);
        inventoryItemCounts = SortByName(inventoryItemCounts, true);
        inventoryHandler.inventoryItemCounts = inventoryItemCounts;
        inventoryHandler.RenderItems();
    }
}