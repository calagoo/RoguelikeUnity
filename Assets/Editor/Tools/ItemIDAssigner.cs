#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class ItemIDAssigner
{
    [MenuItem("Tools/Inventory/Auto-Assign Item IDs")]
    public static void AssignItemIDs()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        var items = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)))
            .OrderBy(i => i.name) // Optional: consistent order
            .ToList();

        int id = 0;
        foreach (var item in items)
        {
            if (item.id != id)
            {
                Undo.RecordObject(item, "Assign Item ID");
                item.id = id;
                EditorUtility.SetDirty(item);
            }
            id++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Assigned itemIDs to {items.Count} items.");
    }
}
#endif
