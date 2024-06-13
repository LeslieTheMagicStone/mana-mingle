using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Backpack : NetworkBehaviour
{
    public List<SpellBase> spells { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;

        spells = new();
    }

    public void AddSpell(SpellBase spell)
    {
        spells.Add(spell);
        GameLogic.instance.AddSpellPreview(spell);
    }
}
