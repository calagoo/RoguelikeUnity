using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHandler : MonoBehaviour
{
    public ItemData activeItemData;
    public ThrowLogic activeItemLogic;
    // Start is called before the first frame update
    public void ThrowItem(Vector3 target)
    {
    if (activeItemLogic == null)
    {
        Debug.LogWarning("❌ activeItemLogic is not assigned.");
        return;
    }

    if (activeItemData == null)
    {
        Debug.LogWarning("❌ activeItemData is not assigned.");
        return;
    }
    activeItemLogic.Execute(gameObject, target, activeItemData);
    }

}
