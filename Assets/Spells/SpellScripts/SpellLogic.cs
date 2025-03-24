using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellLogic : ScriptableObject
{
    public abstract void Execute(GameObject caster, Vector3 target, SpellData data);
}
