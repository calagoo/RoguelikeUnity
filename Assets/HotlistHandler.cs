using UnityEngine;

public class HotlistHandler : MonoBehaviour
{
    public ItemData[] hotlistItems; // Array to hold the hotlist items
    public GameObject[] hotlistSlots; // Array to hold the hotlist slots
    public Sprite hotlistUnselectedIcon;
    public Sprite hotlistSelectedIcon;

    private void Start()
    {
        // Initialize the hotlist with empty slots
        for (int i = 0; i < hotlistSlots.Length; i++)
        {
            hotlistSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = hotlistUnselectedIcon;
        }
    }

    public void SelectHotBar(int slotIndex)
    {
        for (int i = 0; i < hotlistSlots.Length; i++)
        {
            if (i == slotIndex)
            {
                hotlistSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = hotlistSelectedIcon;   
            }
            else
            {
                hotlistSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = hotlistUnselectedIcon;
            }
        }
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
