using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public class InventoryHandler : MonoBehaviour
{
    public GameObject Player;
    private List<int> inventoryItems = new();
    public Dictionary<int, int> inventoryItemCounts = new();
    public ItemDatabase itemDatabase;
    public VisualElement ui;
    public VisualElement InventoryMenu;

    public class ItemAttributes
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Rarity { get; set; }
        public float Weight { get; set; }
        public int Value { get; set; }
        public int ID { get; set; }
    }
    public MultiColumnListView inventoryListView;
    public List<ItemAttributes> inventoryData = new();

    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        inventoryListView = ui.Q<MultiColumnListView>("InventoryListView");

        inventoryListView.showBoundCollectionSize = false;
        inventoryListView.selectionType = SelectionType.Multiple;
        inventoryListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

        // Setup item source
        inventoryListView.itemsSource = inventoryData;

        // Sorting Columns
        inventoryListView.sortingMode = ColumnSortingMode.Default;

        // Setup columns
        inventoryListView.columns["ItemName"].makeCell = () => new Label();
        inventoryListView.columns["ItemName"].bindCell = (e, i) =>
            (e as Label).text = inventoryData[i].Name;
        inventoryListView.columns["ItemName"].comparison = (a, b) =>
            inventoryData[a].Name.CompareTo(inventoryData[b].Name);

        inventoryListView.columns["ItemQuantity"].makeCell = () => new Label();
        inventoryListView.columns["ItemQuantity"].bindCell = (e, i) =>
            (e as Label).text = inventoryData[i].Quantity.ToString();
        inventoryListView.columns["ItemQuantity"].comparison = (a, b) =>
            inventoryData[a].Quantity.CompareTo(inventoryData[b].Quantity);

        inventoryListView.columns["ItemRarity"].makeCell = () => new Label();
        inventoryListView.columns["ItemRarity"].bindCell = (e, i) =>
            (e as Label).text = GetRarity(inventoryData[i].Rarity);
        inventoryListView.columns["ItemRarity"].comparison = (a, b) =>
            inventoryData[a].Rarity.CompareTo(inventoryData[b].Rarity);

        inventoryListView.columns["ItemWeight"].makeCell = () => new Label();
        inventoryListView.columns["ItemWeight"].bindCell = (e, i) =>
            (e as Label).text = inventoryData[i].Weight.ToString();
        inventoryListView.columns["ItemWeight"].comparison = (a, b) =>
            inventoryData[a].Weight.CompareTo(inventoryData[b].Weight);

        inventoryListView.columns["ItemValue"].makeCell = () => new Label();
        inventoryListView.columns["ItemValue"].bindCell = (e, i) =>
            (e as Label).text = inventoryData[i].Value.ToString();
        inventoryListView.columns["ItemValue"].comparison = (a, b) =>
            inventoryData[a].Value.CompareTo(inventoryData[b].Value);
        
        inventoryListView.columnSortingChanged += OnSortChanged;

        if (itemDatabase == null)
            itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
    }

    string GetRarity(int rarityInt)
    {
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
            case 5:
                rarityString = "Celestial";
                break;
        }
        return rarityString;
    }

    public void OnSortChanged()
    {
        if (inventoryListView.sortedColumns.Count() == 0)
            return;

        var sortedColumns = inventoryListView.sortedColumns;
        foreach (var column in sortedColumns.Reverse())
        {
            var columnName = column.columnName;
            var direction = column.direction;
            SortColumn(columnName, direction);
        }

        // Manually sync the itemsSource (not needed if it's already bound to inventoryData)
        inventoryListView.Rebuild();
    }

    void SortColumn(string columnName, SortDirection direction)
    {
        // Sort the inventoryData based on the column name and direction
        // Replace inventoryItems with the sorted data

        inventoryData.Sort((a, b) =>
        {
            return columnName switch
            {
                "ItemName" => direction == SortDirection.Ascending
                                        ? a.Name.CompareTo(b.Name)
                                        : b.Name.CompareTo(a.Name),
                "ItemQuantity" => direction == SortDirection.Ascending
                                        ? a.Quantity.CompareTo(b.Quantity)
                                        : b.Quantity.CompareTo(a.Quantity),
                "ItemRarity" => direction == SortDirection.Ascending
                                        ? a.Rarity.CompareTo(b.Rarity)
                                        : b.Rarity.CompareTo(a.Rarity),
                "ItemWeight" => direction == SortDirection.Ascending
                                        ? a.Weight.CompareTo(b.Weight)
                                        : b.Weight.CompareTo(a.Weight),
                "Value" => direction == SortDirection.Ascending
                                        ? a.Value.CompareTo(b.Value)
                                        : b.Value.CompareTo(a.Value),
                _ => 0,
            };
        });

        // Update the inventoryItems with the sorted data
        inventoryItems.Clear();
        foreach (var item in inventoryData)
        {
            for (int i = 0; i < item.Quantity; i++)
            {
                inventoryItems.Add(item.ID);
            }
        }
        CountUniqueItems(); // Recalculate unique items after sorting
    }

    public void AddItem(ItemData item)
    {
        inventoryItems.Add(item.id);
        CountUniqueItems();
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
        UpdateInventoryListView();
    }

    void UpdateInventoryListView()
    {
        inventoryData.Clear();
        foreach (var pair in inventoryItemCounts)
        {
            var item = itemDatabase.GetItemByID(pair.Key);
            inventoryData.Add(new ItemAttributes
            {
                Name = item.itemName,
                Quantity = pair.Value,
                Rarity = item.rarity,
                Weight = item.weight,
                Value = item.baseValue,
                ID = item.id
            });
        }
        inventoryListView.Rebuild(); // Triggers refresh
    }

    public void RemoveItem(IEnumerable<ItemAttributes> selectedItems, int count = 1)
    {
        // Get ID's from selected items
        foreach (ItemAttributes item in selectedItems)
        {
            int id = item.ID;
            if (inventoryItemCounts.ContainsKey(id))
            {
                inventoryItems.Remove(id);
                GameObject instance = Instantiate(itemDatabase.GetItemByID(id).prefab);
                instance.transform.SetPositionAndRotation(Player.transform.position, Player.transform.rotation);
            }
        }
        CountUniqueItems();
        OnSortChanged(); // Sort the inventory items before counting unique items
    }

    public List<ItemAttributes> GetSelectedItems()
    {
        var selectedItems = new List<ItemAttributes>();

        foreach (int index in inventoryListView.selectedIndices)
        {
            if (index >= 0 && index < inventoryListView.itemsSource.Count)
            {
                if (inventoryListView.itemsSource[index] is ItemAttributes selected)
                {
                    selectedItems.Add(selected);
                }
            }
        }
        return selectedItems.Count > 0 ? selectedItems : null;
    }

    public void ClearSelectedItems()
    {
        inventoryListView.SetSelection(new List<int>()); // Clear selection
    }

    public void AddToHotlist(ItemAttributes item, int hotlistSlot)
    {
        // Add item to hotlist slot
        // hotlistHandler.AddToHotlist(item, hotlistSlot);
        Debug.Log($"Added {item.Name} to hotlist slot {hotlistSlot}");
    }

    public void UseItem(ItemAttributes item)
    {
        // Use item logic here
        Debug.Log("Item used " + item.Name);
    }
    public void InspectItem(ItemAttributes item)
    {
        // Inspect item logic here
        Debug.Log("Item inspected " + item.Name);
    }
}
