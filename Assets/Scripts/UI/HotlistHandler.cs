using UnityEngine;
using UnityEngine.UIElements;

public class HotlistHandler : MonoBehaviour
{
    public VisualElement ui;
    public VisualElement hotlistContainer;
    public VisualElement[] hotlistSlots; // Array to hold the hotlist slots (UI elements)
    public ItemData[] hotlistItems; // Array to hold the hotlist items (data)
    public int selectedSlotIndex = -1; // Index of the currently selected slot
    readonly int slotCount = 10; // Number of slots in the hotlist

    private void Awake()
    {
        hotlistSlots = new VisualElement[slotCount];
        hotlistItems = new ItemData[slotCount];

        ui = GetComponent<UIDocument>().rootVisualElement;
        hotlistContainer = ui.Q<VisualElement>("Hotlist");

        for (int i = 0; i < slotCount; i++)
        {
            hotlistSlots[i] = hotlistContainer.Q<VisualElement>("Hotlist" + i);
            hotlistItems[i] = null;
        }
    }

    public void SelectHotBar(int slotIndex)
    {
        if (slotIndex == selectedSlotIndex)
        {
            // If the same slot is clicked again, deselect it
            hotlistSlots[slotIndex].RemoveFromClassList("selected");
            hotlistSlots[slotIndex].AddToClassList("unselected");
            selectedSlotIndex = -1;
            return;
        }

        for (int i = 0; i < slotCount; i++)
        {
            if (i == slotIndex)
            {
                if (hotlistSlots[i].ClassListContains("unselected"))
                {
                    hotlistSlots[i].RemoveFromClassList("unselected");
                }
                hotlistSlots[i].AddToClassList("selected");
            }
            else
            {
                if (hotlistSlots[i].ClassListContains("selected"))
                {
                    hotlistSlots[i].RemoveFromClassList("selected");
                }
                hotlistSlots[i].AddToClassList("unselected");
            }
        }
        selectedSlotIndex = slotIndex;
    }

    public void SetHotBar(int slotIndex, ItemData itemData)
    {
        hotlistItems[slotIndex] = itemData;
    }

    // public void AddItemToHotlist(GameObject item, int slotIndex)
    // {
    //     if (slotIndex < 0 || slotIndex >= hotlistSlots.Length)
    //     {
    //         Debug.LogWarning("Invalid slot index.");
    //         return;
    //     }

    //     // Check if the slot is already occupied
    //     if (hotlistSlots[slotIndex].activeSelf)
    //     {
    //         Debug.LogWarning("Slot is already occupied. Please choose another slot.");
    //         return;
    //     }

    //     // Add the item to the hotlist and activate the corresponding slot
    //     hotlistItems[slotIndex] = item;
    //     hotlistSlots[slotIndex].SetActive(true);
    // }

    // public void RemoveItemFromHotlist(int slotIndex)
    // {
    //     if (slotIndex < 0 || slotIndex >= hotlistSlots.Length)
    //     {
    //         Debug.LogWarning("Invalid slot index.");
    //         return;
    //     }

    //     // Remove the item from the hotlist and deactivate the corresponding slot
    //     hotlistItems[slotIndex] = null;
    //     hotlistSlots[slotIndex].SetActive(false);
    // }
}
