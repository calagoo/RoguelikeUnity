using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUIRotation : MonoBehaviour
{
    // This script rotates the UI over the NPC to always face the player
    // and to remain upright if the enemy falls
    public GameObject player;
    public GameObject npcUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        npcUI.transform.LookAt(player.transform);
        npcUI.transform.Rotate(0, 180, 0);
    }
}
