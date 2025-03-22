using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    // public content
    public GameObject content;
    public GameObject itemPanel;
    private List<Item> inventoryItems = new List<Item>();
    private Dictionary<string, int> inventoryItemCounts = new Dictionary<string, int>();
    private Dictionary<string, Item> itemDataByName = new Dictionary<string, Item>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // RenderItems();
    }

    public void AddItem(Item item)
    {
        // Add the item to the inventoryItems list
        inventoryItems.Add(item.GetComponent<Item>());

        // Add the item to the inventoryItemCounts dictionary
        CountUniqueItems();
        RenderItems();
    }

    void CountUniqueItems()
    {
        inventoryItemCounts.Clear();
        itemDataByName.Clear();
        foreach (Item item in inventoryItems)
        {
            if (inventoryItemCounts.ContainsKey(item.itemName))
            {
                inventoryItemCounts[item.itemName]++;
            }
            else
            {
                inventoryItemCounts.Add(item.itemName, 1);
                itemDataByName.Add(item.itemName, item);
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
        foreach (KeyValuePair<string, int> item in inventoryItemCounts)
        {
            CreateInventoryItemText(itemDataByName[item.Key], item.Value);
        }
    }

    void CreateInventoryItemText(Item item, int count)
    {
        if (content == null)
        {
            return;
        }

        // Create a panel for the item
        GameObject newItemPanel = Instantiate(itemPanel, content.transform);

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

        nameText.rectTransform.sizeDelta = new Vector2(600, 0); // Vertical Is inherited
        quantityText.rectTransform.sizeDelta = new Vector2(150, 50);
        rarityText.rectTransform.sizeDelta = new Vector2(300, 50);
        weightText.rectTransform.sizeDelta = new Vector2(200, 50);
        valueText.rectTransform.sizeDelta = new Vector2(200, 50);

        // Set the text to the item's name
        nameText.text = item.itemName;
        nameText.overflowMode = TextOverflowModes.Ellipsis;
        nameText.enableAutoSizing = true;
        nameText.fontSizeMin = 10;
        nameText.fontSizeMax = 48;
        nameText.color = Color.white;
        nameText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's quantity
        quantityText.text = count.ToString();
        quantityText.overflowMode = TextOverflowModes.Ellipsis;
        quantityText.enableAutoSizing = true;
        quantityText.fontSizeMin = 10;
        quantityText.fontSizeMax = 48;
        quantityText.color = Color.white;
        quantityText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's rarity
        int rarityInt = item.itemRarity;
        string rarityString = "";
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
        rarityText.enableAutoSizing = true;
        rarityText.fontSizeMin = 10;
        rarityText.fontSizeMax = 48;
        rarityText.color = Color.white;
        rarityText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's weight
        weightText.text = item.itemWeight.ToString();
        weightText.overflowMode = TextOverflowModes.Ellipsis;
        weightText.enableAutoSizing = true;
        weightText.fontSizeMin = 10;
        weightText.fontSizeMax = 48;
        weightText.color = Color.white;
        weightText.alignment = TextAlignmentOptions.Center;

        // Set the text to the item's value
        valueText.text = item.itemValue.ToString();
        valueText.overflowMode = TextOverflowModes.Ellipsis;
        valueText.enableAutoSizing = true;
        valueText.fontSizeMin = 10;
        valueText.fontSizeMax = 48;
        valueText.color = Color.white;
        valueText.alignment = TextAlignmentOptions.Center;
    }
}
