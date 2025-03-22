using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainInventory : MonoBehaviour
{
    public GameObject inventoryParent;
    public Button[] tabButtons;
    public GameObject[] tabPanels;

    // Start is called before the first frame update
    void Start()
    {
        inventoryParent.SetActive(false);
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SelectPanel(index));
        }

        // Select first tab by default
        SelectPanel(0);
        tabButtons[0].Select();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SelectPanel(int index)
    {
        for (int i = 0; i < tabPanels.Length; i++)
        {
            if (i == index)
                tabPanels[i].SetActive(true);
            else
                tabPanels[i].SetActive(false);
        }
    }

    public void OpenInventory()
    {
        if (inventoryParent.activeSelf)
        {
            // Resume the game
            Time.timeScale = 1;

            // Lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryParent.SetActive(false);
        }
        else
        {
            // Slow down the game
            Time.timeScale = 0.1f;

            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryParent.SetActive(true);
        }
    }
}
