using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaunchProjectile : MonoBehaviour
{
    public SpellHandler spellHandler;
    public ThrowHandler throwHandler;
    public ItemDatabase itemDatabase;
    public bool isSpell;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnLaunchProjectile(int ID)
    {
        IGameAsset asset = itemDatabase.GetByID(ID);
        isSpell = asset is SpellData;
        if (isSpell)
        {
            LaunchSpell(ID);
        }
        else
        {
            LaunchItem(ID);
        }
    }

    void LaunchSpell(int ID)
    {
        spellHandler.activeSpellData = itemDatabase.GetSpellByID(ID);
        spellHandler.activeSpellLogic = spellHandler.activeSpellData.spellLogic;
        Vector3 target = transform.position + transform.forward * 2;
        spellHandler.CastSpell(target);
    }

    void LaunchItem(int ID)
    {
        throwHandler.activeItemData = itemDatabase.GetItemByID(ID);
        throwHandler.activeItemLogic = throwHandler.activeItemData.throwLogic;
        Vector3 target = transform.position + transform.forward * 2;
        throwHandler.ThrowItem(target);

    }
}
