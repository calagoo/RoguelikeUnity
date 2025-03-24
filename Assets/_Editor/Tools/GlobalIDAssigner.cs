#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class GlobalIDAssigner : Editor
{
    private const string itemPath = "Assets/ScriptableObjects/Items";
    private const string spellPath = "Assets/ScriptableObjects/Spells";

    [MenuItem("Tools/Inventory/Assign Global IDs to Items & Spells")]
    public static void AssignGlobalIDs()
    {
        // Load all ItemData assets
        string[] itemGUIDs = AssetDatabase.FindAssets("t:ItemData", new[] { itemPath });
        ItemData[] itemAssets = itemGUIDs
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(asset => asset != null)
            .ToArray();

        // Load all SpellData assets
        string[] spellGUIDs = AssetDatabase.FindAssets("t:SpellData", new[] { spellPath });
        SpellData[] spellAssets = spellGUIDs
            .Select(guid => AssetDatabase.LoadAssetAtPath<SpellData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(asset => asset != null)
            .ToArray();

        int globalID = 0;

        // Assign IDs to items
        foreach (var item in itemAssets)
        {
            Undo.RecordObject(item, "Assign Global Item ID");
            item.id = globalID++;
            EditorUtility.SetDirty(item);
        }

        // Assign IDs to spells
        foreach (var spell in spellAssets)
        {
            Undo.RecordObject(spell, "Assign Global Spell ID");
            spell.id = globalID++;
            EditorUtility.SetDirty(spell);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Assigned global IDs:\n🧱 Items: {itemAssets.Length}\n✨ Spells: {spellAssets.Length}\n📦 Total Assigned: {globalID}");
    }
}
#endif
