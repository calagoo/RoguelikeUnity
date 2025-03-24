using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    public float strength;
    public GameObject player;
    public InventoryHandler inventory;

    // Start is called before the first frame update
    void Start()
    {
        strength = PlayerStats.Instance.strength;
    }

    public void PickUpItem(float grabRange)
    {
        LayerMask layerMask = LayerMask.GetMask("Player");
        int mask = ~layerMask;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, grabRange, mask)) // Ignore player layer
        {
            GameObject itemGO = hit.collider.gameObject;
            if (itemGO.layer == 7)
            {
                ItemInstance itemInstance = itemGO.GetComponent<ItemInstance>();
                ItemData itemData = itemInstance.GetItemData();
                if (itemData != null)
                {
                    inventory.AddItem(itemData);
                    Destroy(itemGO);
                }
            }
        }
    }
}
