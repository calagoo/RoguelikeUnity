using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    public string resourceFolder = "Items/Models"; // Folder where the items are stored
    public GameObject[] items; // Array of items to spawn
    // Start is called before the first frame update
    void Start()
    {
        items = Resources.LoadAll<GameObject>(resourceFolder);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        // Get a random item from the array
        GameObject item = items[Random.Range(0, items.Length)];
        // Get a random position
        Vector3 position = new Vector3(Random.Range(-10, 10), 3, Random.Range(-10, 10));
        // Instantiate the item
        Instantiate(item, position, item.transform.rotation);
    }
}
