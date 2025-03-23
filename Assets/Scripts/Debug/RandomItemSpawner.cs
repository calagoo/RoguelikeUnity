using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    // Load assets from Assets/ScriptableObjects/Items
    public GameObject[] items;
    public ItemDatabase itemDatabase;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ItemData item = itemDatabase.GetRandomItem();
            Instantiate(item.prefab, transform.position, Quaternion.identity);
        }
    }
}
