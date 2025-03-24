#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class ItemDatabaseUpdater : Editor
{
    private const string itemPath = "Assets/ScriptableObjects/Items";
    private const string spellPath = "Assets/ScriptableObjects/Spells";
    private const string databasePath = "Assets/Resources/Databases/ItemDatabase.asset";

    [MenuItem("Tools/Inventory/Update ItemDatabase With All Items and Spells")]
    public static void UpdateItemDatabase()
    {
        // Load or create the ItemDatabase
        ItemDatabase database = AssetDatabase.LoadAssetAtPath<ItemDatabase>(databasePath);
        if (database == null)
        {
            Debug.LogWarning("ItemDatabase not found, creating a new one.");
            database = ScriptableObject.CreateInstance<ItemDatabase>();
            Directory.CreateDirectory("Assets/Resources/Databases");
            AssetDatabase.CreateAsset(database, databasePath);
        }

        Undo.RecordObject(database, "Update ItemDatabase");

        // Find all items and spells
        var itemGUIDs = AssetDatabase.FindAssets("t:ItemData", new[] { itemPath });
        var spellGUIDs = AssetDatabase.FindAssets("t:SpellData", new[] { spellPath });

        var allItems = itemGUIDs
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(asset => asset != null)
            .ToList();

        var allSpells = spellGUIDs
            .Select(guid => AssetDatabase.LoadAssetAtPath<SpellData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(asset => asset != null)
            .ToList();

        // Assign lists
        database.items = allItems;
        database.spells = allSpells;

        // Combine into unified list
        database.gameAssets = new List<IGameAsset>();
        database.gameAssets.AddRange(allItems);
        database.gameAssets.AddRange(allSpells);

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log($"✅ ItemDatabase updated:\n🧱 Items: {allItems.Count}\n✨ Spells: {allSpells.Count}\n📦 Total Assets: {database.gameAssets.Count}");
    }
}
#endif
