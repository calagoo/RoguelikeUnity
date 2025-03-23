#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class ItemInstanceAssigner
{
    [MenuItem("Tools/Inventory/Auto-Assign ItemInstance to Prefabs")]
    public static void AssignItemInstances()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        int updated = 0;

        foreach (string guid in guids)
        {
            ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid));
            if (itemData == null || itemData.prefab == null)
                continue;

            string prefabPath = AssetDatabase.GetAssetPath(itemData.prefab);
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

            ItemInstance instance = prefabRoot.GetComponent<ItemInstance>();
            if (instance == null)
            {
                instance = prefabRoot.AddComponent<ItemInstance>();
                Debug.Log($"✅ Added ItemInstance to prefab: {itemData.itemName}");
            }

            instance.itemData = itemData;

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            updated++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"🏁 Finished assigning ItemInstance to {updated} prefabs.");
    }
}
#endif
