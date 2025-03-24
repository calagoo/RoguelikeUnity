using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDatabase", menuName = "Spells/Spell Database")]
public class SpellDatabase : ScriptableObject
{
    public List<SpellData> allSpells;

    public SpellData GetSpellByName(string name)
    {
        return allSpells.Find(s => s.spellName == name);
    }
    public SpellData GetRandomSpell()
    {
        int randomIndex = Random.Range(0, allSpells.Count);
        return allSpells[randomIndex];
    }
    public SpellData GetByID(int id)
    {
        return allSpells[id];
    }
}
