using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{
    public SpellData activeSpellData;
    public SpellLogic activeSpellLogic;
    public PlayerMana playerMana;
    void Awake()
    {
    }

    public void CastSpell(Vector3 target)
    {
        if (playerMana.Mana < activeSpellData.manaCost)
        {
            Debug.Log("Not enough mana to cast spell");
            return;
        }
        float cd = activeSpellData.cooldown;
        activeSpellLogic.Execute(gameObject, target, activeSpellData);
        playerMana.TakeDamage(activeSpellData.manaCost);
    }
}