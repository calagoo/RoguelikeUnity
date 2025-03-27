using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Controls the main player stats
    // Main stats (initial values) (average)
    // Strength (2-10) (4)
    // Intelligence (2-10) (4)
    // Constitution (2-10) (4)
    // Dexterity (2-10) (4)
    // Charisma (2-10) (4)
    //
    // 10 == Max of natural human ability
    // 2 == ~minimum for a human
    //
    // Strength of 4 ~= lifting 100kg, 220lbs
    // Strength of 4 ~= 1000 force (newtons?)
    // each strength adds 25kg, 250N
    // strength of 100
    //
    // Intelligence == Mana
    // 

    public static PlayerStats Instance { get; private set; }

    public StatGenerator statGenerator;
    public int strength;
    public int intelligence;
    public int constitution;
    public int dexterity;
    public int charisma;
    public int level = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist between scenes

        var statGen = GetComponent<StatGenerator>();
        Dictionary<string, int> stats = statGen.RandomizeStats();
        foreach (var stat in stats)
        {
            switch (stat.Key)
            {
                case "strength":
                    strength = stat.Value;
                    break;
                case "intelligence":
                    intelligence = stat.Value;
                    break;
                case "constitution":
                    constitution = stat.Value;
                    break;
                case "dexterity":
                    dexterity = stat.Value;
                    break;
                case "charisma":
                    charisma = stat.Value;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
