using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    public ItemData itemData;

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
    }

    public ItemData GetItemData()
    {
        return itemData;
    }
}
