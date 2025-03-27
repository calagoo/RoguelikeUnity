using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    public GameObject Player;
    public GameObject content;
    public GameObject itemPanel;
    private List<int> inventoryItems = new List<int>();
    private Dictionary<int, int> inventoryItemCounts = new Dictionary<int, int>();
    // private Dictionary<string, Item> itemDataByName = new Dictionary<string, Item>();

    public class InventoryEntry
    {
        public string itemName;
        public int itemQuantity;
        public int itemRarity;
        public int itemWeight;
        public int itemValue;
        public GameObject prefabReference;
    }

    List<InventoryEntry> inventoryEntries = new List<InventoryEntry>();

    private int fontSize = 43;


    public ItemDatabase itemDatabase;
    void Awake()
    {
        if (itemDatabase == null)
            itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
    }

    public void AddItem(ItemData item)
    {
        // Add the item to the inventoryItems list
        inventoryItems.Add(item.id);

        // Add the item to the inventoryItemCounts dictionary
        CountUniqueItems();

        // Add the item to the inventoryEntries list
        // AddEntry(inventoryItemCounts, itemDataByName);

        // Set a sorttype if later
        // int sortType = 1;
        // SortInventoryItems(inventoryItemCounts, itemDataByName, sortType);

        // Render the items
        RenderItems();
    }

    void CountUniqueItems()
    {
        inventoryItemCounts.Clear();
        foreach (int id in inventoryItems)
        {
            if (inventoryItemCounts.ContainsKey(id))
            {
                inventoryItemCounts[id]++;
            }
            else
            {
                inventoryItemCounts.Add(id, 1);
            }
        }
    }

    void RenderItems()
    {
        // Clear the content panel
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (KeyValuePair<int, int> item in inventoryItemCounts)
        {
            CreateInventoryItemText(itemDatabase.GetItemByID(item.Key), item.Value);
        }
    }

    void CreateInventoryItemText(ItemData item, int count)
    {
        if (content == null)
        {
            return;
        }

        // Create a panel for the item
        GameObject newItemPanel = Instantiate(itemPanel, content.transform);
        newItemPanel.GetComponent<Image>().color = Color.clear;
        // Add Button to the panel
        Button button = newItemPanel.AddComponent<Button>();
        // button.onClick.AddListener(() => SelectItem(item.itemName));
        button.onClick.AddListener(() => HighlightItem(newItemPanel));

        // Create a new GameObject for the UI text
        GameObject itemNameObj = new GameObject("ItemName");
        GameObject itemQuantityObj = new GameObject("ItemQuantity");
        GameObject itemRarityObj = new GameObject("ItemRarity");
        GameObject itemWeightObj = new GameObject("ItemWeight");
        GameObject itemValueObj = new GameObject("ItemValue");

        // Set it as a child of the UI content panel
        itemNameObj.transform.SetParent(newItemPanel.transform);
        itemQuantityObj.transform.SetParent(newItemPanel.transform);
        itemRarityObj.transform.SetParent(newItemPanel.transform);
        itemWeightObj.transform.SetParent(newItemPanel.transform);
        itemValueObj.transform.SetParent(newItemPanel.transform);

        // Add a TextMeshProUGUI component
        TextMeshProUGUI nameText = itemNameObj.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI quantityText = itemQuantityObj.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI rarityText = itemRarityObj.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI weightText = itemWeightObj.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI valueText = itemValueObj.AddComponent<TextMeshProUGUI>();

        // Set the size of the text
        nameText.rectTransform.sizeDelta = new Vector2(600, 0); // Vertical Is inherited
        quantityText.rectTransform.sizeDelta = new Vector2(160, 50);
        rarityText.rectTransform.sizeDelta = new Vector2(350, 50);
        weightText.rectTransform.sizeDelta = new Vector2(180, 50);
        valueText.rectTransform.sizeDelta = new Vector2(180, 50);

        // Set the scale of the text
        nameText.rectTransform.localScale = new Vector3(1, 1, 1);
        quantityText.rectTransform.localScale = new Vector3(1, 1, 1);
        rarityText.rectTransform.localScale = new Vector3(1, 1, 1);
        weightText.rectTransform.localScale = new Vector3(1, 1, 1);
        valueText.rectTransform.localScale = new Vector3(1, 1, 1);

        // Set the text to the item's name
        nameText.text = item.itemName;
        nameText.overflowMode = TextOverflowModes.Ellipsis;
        nameText.fontSize = fontSize;
        nameText.color = Color.white;
        nameText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's quantity
        quantityText.text = count.ToString();
        quantityText.overflowMode = TextOverflowModes.Ellipsis;
        quantityText.fontSize = fontSize;
        quantityText.color = Color.white;
        quantityText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's rarity
        int rarityInt = item.rarity;
        string rarityString = "Trash";
        switch (rarityInt) // Slightly different rarities than loot boxes
        {
            case 0:
                rarityString = "Common";
                break;
            case 1:
                rarityString = "Uncommon";
                break;
            case 2:
                rarityString = "Rare";
                break;
            case 3:
                rarityString = "Epic";
                break;
            case 4:
                rarityString = "Legendary";
                break;
            default:
                rarityString = "Celestial";
                break;
        }

        rarityText.text = rarityString;
        rarityText.overflowMode = TextOverflowModes.Ellipsis;
        rarityText.fontSize = fontSize;
        rarityText.color = Color.white;
        rarityText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's weight
        weightText.text = item.weight.ToString();
        weightText.overflowMode = TextOverflowModes.Ellipsis;
        weightText.fontSize = fontSize;
        weightText.color = Color.white;
        weightText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's value
        valueText.text = item.baseValue.ToString();
        valueText.overflowMode = TextOverflowModes.Ellipsis;
        valueText.fontSize = fontSize;
        valueText.color = Color.white;
        valueText.alignment = TextAlignmentOptions.Center;
    }

    // void SortInventoryItems(Dictionary<string, int> itemCount, Dictionary<string, Item> itemData, int sortType)
    // {
    //     // Sort types:
    //     // 0 = Name (A-Z)
    //     // 1 = Name (Z-A)
    //     // 2 = Quantity (Low-High)
    //     // 3 = Quantity (High-Low)
    //     // 4 = Rarity (Low-High)
    //     // 5 = Rarity (High-Low)
    //     // 6 = Weight (Low-High)
    //     // 7 = Weight (High-Low)
    //     // 8 = Value (Low-High)
    //     // 9 = Value (High-Low)
    //     switch (sortType)
    //     {
    //         case 0:
    //             inventoryEntries.Sort((x, y) => x.itemName.CompareTo(y.itemName));
    //             break;
    //         case 1:
    //             inventoryEntries.Sort((x, y) => y.itemName.CompareTo(x.itemName));
    //             break;
    //         case 2:
    //             inventoryEntries.Sort((x, y) => x.itemQuantity.CompareTo(y.itemQuantity));
    //             break;
    //         case 3:
    //             inventoryEntries.Sort((x, y) => y.itemQuantity.CompareTo(x.itemQuantity));
    //             break;
    //         case 4:
    //             inventoryEntries.Sort((x, y) => x.itemRarity.CompareTo(y.itemRarity));
    //             break;
    //         case 5:
    //             inventoryEntries.Sort((x, y) => y.itemRarity.CompareTo(x.itemRarity));
    //             break;
    //         case 6:
    //             inventoryEntries.Sort((x, y) => x.itemWeight.CompareTo(y.itemWeight));
    //             break;
    //         case 7:
    //             inventoryEntries.Sort((x, y) => y.itemWeight.CompareTo(x.itemWeight));
    //             break;
    //         case 8:
    //             inventoryEntries.Sort((x, y) => x.itemValue.CompareTo(y.itemValue));
    //             break;
    //         case 9:
    //             inventoryEntries.Sort((x, y) => y.itemValue.CompareTo(x.itemValue));
    //             break;
    //         default:
    //             Debug.LogWarning("Invalid sort type selected: " + sortType);
    //             break;
    //     }

    //     // Put it back into the itemCount dictionary
    //     itemCount.Clear();
    //     itemData.Clear();
    //     foreach (InventoryEntry entry in inventoryEntries)
    //     {
    //         itemCount.Add(entry.itemName, entry.itemQuantity);
    //         itemData.Add(entry.itemName, entry.itemData);
    //     }
    // }

    // void AddEntry(Dictionary<string, int> itemCount, Dictionary<string, Item> itemData)
    // {


    //     // Clear the inventoryEntries list
    //     inventoryEntries.Clear();

    //     // First add items to the inventoryEntries list
    //     foreach (KeyValuePair<string, int> item in itemCount)
    //     {
    //         var model = itemData[item.Key].prefabReference;

    //         InventoryEntry entry = new InventoryEntry();
    //         entry.itemData = itemData[item.Key];
    //         entry.itemName = item.Key;
    //         entry.itemQuantity = item.Value;
    //         entry.itemRarity = itemData[item.Key].itemRarity;
    //         entry.itemWeight = itemData[item.Key].itemWeight;
    //         entry.itemValue = itemData[item.Key].itemValue;
    //         entry.prefabReference = itemData[item.Key].prefabReference;
    //         inventoryEntries.Add(entry);
    //     }
    // }

    void SelectItem(string itemName)
    {
        Debug.Log("Selected item: " + itemName);
    }

    void RemoveItem(int id, int count = 1)
    {
        // Get the item from the inventoryEntries list
        // InventoryEntry entry = inventoryEntries.Find(x => x.itemName == itemName);

        inventoryItems.Remove(id);
        GameObject instance = Instantiate(itemDatabase.GetItemByID(id).prefab);
        instance.transform.SetPositionAndRotation(Player.transform.position, Player.transform.rotation);

        // Instantiate(item.prefabReference, Player.transform.position, Player.transform.rotation);
        CountUniqueItems();
        RenderItems();
    }

    void HighlightItem(GameObject panel)
    {
        panel.GetComponent<Image>().color = Color.green;
    }
}
