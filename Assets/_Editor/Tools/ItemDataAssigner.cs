#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class ItemDataAssigner
{
    private const string modelsPath = "Assets/Items/Models";
    private const string itemDataPath = "Assets/ScriptableObjects/Items";

    [MenuItem("Tools/Inventory/Generate Missing ItemData Assets")]
    public static void AssignItemData()
    {
        // Get all prefab model assets
        string[] modelGuids = AssetDatabase.FindAssets("t:Prefab", new[] { modelsPath });
        var modelPaths = modelGuids
            .Select(AssetDatabase.GUIDToAssetPath)
            .ToList();

        // Get all existing ItemData assets
        string[] itemGuids = AssetDatabase.FindAssets("t:ItemData", new[] { itemDataPath });
        var existingItems = itemGuids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToList();

        int created = 0;

        foreach (string modelPath in modelPaths)
        {
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
            if (model == null) continue;

            bool alreadyExists = existingItems.Any(item => item.prefab == model);

            if (!alreadyExists)
            {
                string itemName = model.name;
                string assetPath = $"{itemDataPath}/{itemName}.asset";

                ItemData newItem = ScriptableObject.CreateInstance<ItemData>();
                newItem.itemName = itemName;
                newItem.prefab = model;
                newItem.description = "New item.";
                newItem.baseValue = 0;
                newItem.weight = 1;
                newItem.rarity = 0;
                newItem.category = ItemData.ItemCategory.Misc;
                newItem.consumableType = ItemData.ConsumableType.None;

                AssetDatabase.CreateAsset(newItem, assetPath);
                Debug.Log($"✅ Created ItemData: {itemName}");
                created++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"🏁 Finished. Created {created} new ItemData assets.");
    }
}
#endif
