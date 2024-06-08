using UnityEngine;
using TMPro;

public class FightCanvas : MonoBehaviour
{
    [SerializeField] private SpellSlotManager spellSlot;
    [SerializeField] private TMP_Text spellName;

    private void Update()
    {
        if (spellSlot.spell == null) spellName.text = "None";
        else spellName.text = spellSlot.spell.spellName;
    }
}
