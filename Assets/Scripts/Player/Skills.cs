using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Skills : MonoBehaviour
{
    public enum PassiveSkills // Player (NPC)
    {
        Stealth, // Footstep quietness, hiding ()
        Perception, // Detecting enemies, traps, on map (Detection range + angle for NPCs)
        Reflex, // (Reaction Time)
        Willpower, // (Likelihood of retreating)
        Precision, // Accuracy (Accuracy)
        Resolve, // (Likelihood of resisting fear)
        UnarmedCombat, //
        ExplosiveHandling, // Determines how much explosives deteriorate
        Explosives, // Higher == Bigger yield
        Shooting, // Archery, Crossbow, Gun
        MeleeCombat, // Sword, Axe, Spear, etc.
        Throwing, // (Accuracy)
        Singing, // For Bard
    }

    public enum FillerSkills // Random Filler skills for player
    {
        Breathing,
        Meditation,
        CastIronRepair,
        SmallTalk,
        Crying,
        Flicking,
        Hammering,
        BikeLubrication,
        CarWashing,
        DogGrooming,
        ZooKeeping,
        HorseRiding,
        FirstPersonShooters,
        RacingSimulators,
        FlightSimulators,
        SandwichMaking,
        Gardening,
        Lightswitching,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Debug.Log("Generating Skills...");
        // Dictionary<PassiveSkills, int> passiveSkills = GeneratePassiveSkills();
        // Dictionary<FillerSkills, int> fillerSkills = GenerateFillerSkills();

        // Debug.Log("Passive Skills: " + string.Join(", ", passiveSkills.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
        // Debug.Log("Filler Skills: " + string.Join(", ", fillerSkills.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Dictionary<PassiveSkills, int> GeneratePassiveSkills()
    {
        // TODO: Add a system to generate skills based on the character's background and traits
        // Generate a 0-8 rating for each skill
        // If rating is 0, dont show in ui... later

        Dictionary<PassiveSkills, int> passiveSkills = new();
        foreach (PassiveSkills skill in System.Enum.GetValues(typeof(PassiveSkills)))
        {
            int rating = Random.Range(0, 5); // 0-8 rating
            passiveSkills.Add(skill, rating);
        }
        return passiveSkills;
    }

    public Dictionary<FillerSkills, int> GenerateFillerSkills()
    {
        // Generate a 0-8 rating for each skill
        // If rating is 0, dont show in ui... later

        Dictionary<FillerSkills, int> fillerSkills = new();
        foreach (FillerSkills skill in System.Enum.GetValues(typeof(FillerSkills)))
        {
            int rating = Random.Range(0, 7); // 0-8 rating
            fillerSkills.Add(skill, rating);
        }
        return fillerSkills;
    }
}
