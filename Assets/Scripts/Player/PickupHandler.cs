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

    public void pickUpItem(float grabRange)
    {
        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, grabRange))
        {
            GameObject itemGO = hit.collider.gameObject;

            if (itemGO.layer == 7)
            {
                Item itemComponent = itemGO.GetComponent<Item>();
                if (itemComponent != null)
                {
                    // Extract item data BEFORE destroying the GameObject
                    inventory.AddItem(itemComponent); // Pass only the data

                    Destroy(itemGO); // Safe now!
                }
            }
        }
    }
}
