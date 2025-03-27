using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
public class EnemyStats : MonoBehaviour
{

    public MobData mobData;
    public int strength;
    public int intelligence;
    public int constitution;
    public int dexterity;
    public int charisma;
    public int level;

    // Start is called before the first frame update
    public void Start()
    {
        // Generate Stats from level and average stats
        List<int> statList = GenerateEnemyStats.Generate(mobData);

        strength = statList[0];
        intelligence = statList[1];
        constitution = statList[2];
        dexterity = statList[3];
        charisma = statList[4];

        level = mobData.mobLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintStats()
    {
        Debug.Log("Strength: " + strength + " Intelligence: " + intelligence + " Constitution: " + constitution + " Dexterity: " + dexterity + " Charisma: " + charisma);
    }
}
