using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMob", menuName = "Misc/Mob")]
public class MobData : ScriptableObject
{
    public enum MobType
    {
        NPC,
        Enemy,
        Pet,
        Boss,
        Hunter,
        Crawler
    }

    [Header("Mob Type")]
    public MobType mobType;
    public string mobName;
    public string mobDescription;


    [Header("Stats")]
    public int strength;
    public int intelligence;
    public int constitution;
    public int dexterity;
    public int charisma;

    public int mobLevel = 0;
}
