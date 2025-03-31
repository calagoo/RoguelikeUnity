using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
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
                InventoryHandler.RemoveItem(selectedItems);
            
        }
    }

    public void OpenInventory()
    {
        if (InventoryMenu.ClassListContains("hide"))
        {
            InventoryMenu.RemoveFromClassList("hide");
            // Slow down the game
            Time.timeScale = 0.1f;

            // Unlock the cursor
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            InventoryMenu.AddToClassList("hide");
            // Resume the game
            Time.timeScale = 1;

            // Lock the cursor
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
}
