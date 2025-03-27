using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GenerateEnemyStats
{
    public static List<int> Generate(MobData mobData)
    {
        // Each stat is the average of the mobs typical stat @ level 1
        // Each level adds 3 stat points to the mob

        Dictionary<string, int> statWeights = new()
        {
            { "strength", mobData.strength },
            { "intelligence", mobData.intelligence },
            { "constitution", mobData.constitution },
            { "dexterity", mobData.dexterity },
            { "charisma", mobData.charisma }
        };

        List<string> statKeys = new();
        foreach (var stat in statWeights)
        {
            for (int i = 0; i < stat.Value; i++)
            {
                statKeys.Add(stat.Key);
            }
        }

        List<int> statList = new() { mobData.strength, mobData.intelligence, mobData.constitution, mobData.dexterity, mobData.charisma };
        for (int i = 0; i < mobData.mobLevel; i++)
        {
            IncreaseRandomStat(statList, statKeys);
        }
        return statList;
    }

    private static void IncreaseRandomStat(List<int> statList, List<string> statKeys)
    {

        int randomStatIndex = UnityEngine.Random.Range(0, statKeys.Count);
        string randomStat = statKeys[randomStatIndex];

        switch (randomStat)
        {
            case "strength":
                statList[0]++;
                break;
            case "intelligence":
                statList[1]++;
                break;
            case "constitution":
                statList[2]++;
                break;
            case "dexterity":
                statList[3]++;
                break;
            case "charisma":
                statList[4]++;
                break;
        }
    }
}