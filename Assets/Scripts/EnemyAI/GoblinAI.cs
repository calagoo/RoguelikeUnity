using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAI : NPCAI
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    readonly NPCAI npcAI;

    // protected override void Start()
    // {
    //     npcAI.moveSpeed = walkSpeed;
    // }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
