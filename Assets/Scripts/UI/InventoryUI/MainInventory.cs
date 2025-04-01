using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
public class MainInventory : MonoBehaviour
{
    public VisualElement ui;
    public VisualElement InventoryMenu;
    public InventoryHandler InventoryHandler;

    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        InventoryMenu = ui.Q<VisualElement>("InventoryMenu");
        InventoryMenu.AddToClassList("hide");
    }

    void Update()
    {
        // If inventory tab is active
        if (InventoryMenu.tabIndex == 0 && !InventoryMenu.ClassListContains("hide")) // Only if inventory tab is active
        {
            List<InventoryHandler.ItemAttributes> selectedItems = InventoryHandler.GetSelectedItems();
            if (Input.GetKeyDown(KeyCode.Q))
                InventoryHandler.RemoveItem(selectedItems); // Drop item
            if (Input.GetKeyDown(KeyCode.F))
                InventoryHandler.UseItem(selectedItems.FirstOrDefault());
            if (Input.GetKeyDown(KeyCode.I))
                InventoryHandler.InspectItem(selectedItems.FirstOrDefault());
            if (selectedItems != null)
                InputHotlist(selectedItems.FirstOrDefault()); // Add to hotlist
        }
        else
        {
            InventoryHandler.ClearSelectedItems(); // Clear selected items if not in inventory tab
        }
    }

    public void OpenInventory()
    {
        if (InventoryMenu.ClassListContains("hide")) // Open
        {
            InventoryHandler.ClearSelectedItems(); // Clear selected items when opening inventory

            InventoryMenu.RemoveFromClassList("hide");
            // Slow down the game
            Time.timeScale = 0.1f;

            // Unlock the cursor
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else // Close
        {
            InventoryMenu.AddToClassList("hide");
            // Resume the game
            Time.timeScale = 1;

            // Lock the cursor
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

    }
    void InputHotlist(InventoryHandler.ItemAttributes item)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            InventoryHandler.AddToHotlist(item, 0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            InventoryHandler.AddToHotlist(item, 1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            InventoryHandler.AddToHotlist(item, 2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            InventoryHandler.AddToHotlist(item, 3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            InventoryHandler.AddToHotlist(item, 4);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            InventoryHandler.AddToHotlist(item, 5);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            InventoryHandler.AddToHotlist(item, 6);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            InventoryHandler.AddToHotlist(item, 7);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            InventoryHandler.AddToHotlist(item, 8);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            InventoryHandler.AddToHotlist(item, 9);
    }
}
