using System.Collections.Generic;
using UnityEngine;

public class StatGenerator : MonoBehaviour
{
    private Dictionary<int, float> statWeights = new Dictionary<int, float>()
    {
        { 2,  1f / 60f },
        { 3,  1f / 16f },
        { 4,  1f / 4f },
        { 5,  1f / 12f },
        { 6,  1f / 80f },
        { 7,  1f / 750f },
        { 8,  1f / 5000f },
        { 9,  1f / 10000f },
        { 10, 1f / 100000f }
    };

    private string[] skillNames = { "strength", "intelligence", "constitution", "dexterity", "charisma" };

    public Dictionary<string, int> RandomizeStats()
    {
        // Clone weights
        Dictionary<int, float> _statWeights = new Dictionary<int, float>(statWeights);

        // Shuffle skills
        List<string> skills = new List<string>(skillNames);
        for (int i = 0; i < skills.Count; i++)
        {
            int rnd = Random.Range(i, skills.Count);
            string temp = skills[i];
            skills[i] = skills[rnd];
            skills[rnd] = temp;
        }

        Dictionary<string, int> stats = new Dictionary<string, int>();

        foreach (string skill in skills)
        {
            int chosenStat = WeightedRandom(_statWeights);
            stats[skill] = chosenStat;

            float average = 0;
            foreach (var val in stats.Values)
                average += val;
            average /= stats.Count;

            // Adjust weights based on average
            List<int> keys = new List<int>(_statWeights.Keys);
            foreach (int key in keys)
            {
                if (key > average)
                {
                    _statWeights[key] *= 1.5f;
                }
                else
                {
                    _statWeights[key] *= 0.7f;
                }
            }
        }

        return stats;
    }

    private int WeightedRandom(Dictionary<int, float> weights)
    {
        float total = 0f;
        foreach (var w in weights.Values)
            total += w;

        float rand = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var kvp in weights)
        {
            cumulative += kvp.Value;
            if (rand <= cumulative)
                return kvp.Key;
        }

        // Fallback (shouldn’t hit this)
        return 2;
    }
}
