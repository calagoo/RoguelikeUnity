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
    public InventorySortingHandler sortingHandler;
    private List<int> inventoryItems = new List<int>();
    public Dictionary<int, int> inventoryItemCounts = new Dictionary<int, int>();
    private readonly int fontSize = 36;
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
        sortingHandler.SortInventoryItems_IH();

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

    public void RenderItems()
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
        newItemPanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.0f);
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
        nameText.verticalAlignment = VerticalAlignmentOptions.Middle;
        nameText.horizontalAlignment = HorizontalAlignmentOptions.Left;

        // Set the text to the item's quantity
        quantityText.text = count.ToString();
        quantityText.overflowMode = TextOverflowModes.Ellipsis;
        quantityText.fontSize = fontSize;
        quantityText.color = Color.white;
        quantityText.verticalAlignment = VerticalAlignmentOptions.Middle;
        quantityText.horizontalAlignment = HorizontalAlignmentOptions.Right;        

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
        rarityText.verticalAlignment = VerticalAlignmentOptions.Middle;
        rarityText.horizontalAlignment = HorizontalAlignmentOptions.Right;

        // Set the text to the item's weight
        weightText.text = item.weight.ToString();
        weightText.overflowMode = TextOverflowModes.Ellipsis;
        weightText.fontSize = fontSize;
        weightText.color = Color.white;
        weightText.verticalAlignment = VerticalAlignmentOptions.Middle;
        weightText.horizontalAlignment = HorizontalAlignmentOptions.Right;

        // Set the text to the item's value
        valueText.text = item.baseValue.ToString();
        valueText.overflowMode = TextOverflowModes.Ellipsis;
        valueText.fontSize = fontSize;
        valueText.color = Color.white;
        valueText.verticalAlignment = VerticalAlignmentOptions.Middle;
        valueText.horizontalAlignment = HorizontalAlignmentOptions.Right;

    }

    void SelectItem(string itemName)
    {
        Debug.Log("Selected item: " + itemName);
    }

    void RemoveItem(int id, int count = 1)
    {
        inventoryItems.Remove(id);
        GameObject instance = Instantiate(itemDatabase.GetItemByID(id).prefab);
        instance.transform.SetPositionAndRotation(Player.transform.position, Player.transform.rotation);

        CountUniqueItems();
        RenderItems();
    }

    void HighlightItem(GameObject panel)
    {
        panel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
    }
}
