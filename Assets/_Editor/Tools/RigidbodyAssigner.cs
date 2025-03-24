#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class RigidbodyAssigner
{
    [MenuItem("Tools/Inventory/Assign Rigidbody to Item Prefabs")]
    public static void AssignRigidbodies()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        var itemDatas = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null && item.prefab != null)
            .ToList();

        int updated = 0;

        foreach (var item in itemDatas)
        {
            GameObject prefab = item.prefab;
            string prefabPath = AssetDatabase.GetAssetPath(prefab);

            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
            Rigidbody rb = prefabRoot.GetComponent<Rigidbody>();

            if (rb == null)
            {
                rb = prefabRoot.AddComponent<Rigidbody>();
                Debug.Log($"✅ Added Rigidbody to: {item.itemName}");
            }

            rb.mass = Mathf.Max(0.1f, item.weight); // prevent 0 mass
            updated++;

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"🏁 Finished assigning Rigidbody to {updated} item prefabs.");
    }
}
#endif
