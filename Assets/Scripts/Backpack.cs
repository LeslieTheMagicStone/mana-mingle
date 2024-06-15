using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Backpack : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
    }

    public void AddSpell(SpellBase spell)
    {
        // print("Adding spell" + spell.displayName + " to backpack.");
        GameLogic.instance.AddSpellPreview(spell.spellVariant);
    }
}
