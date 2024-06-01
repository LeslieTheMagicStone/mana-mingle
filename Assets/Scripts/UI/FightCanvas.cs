using UnityEngine;
using TMPro;

public class FightCanvas : MonoBehaviour
{
    [SerializeField] private SpellSlotManager spellSlot_P1;
    [SerializeField] private TMP_Text spellName_P1;

    [SerializeField] private SpellSlotManager spellSlot_P2;
    [SerializeField] private TMP_Text spellName_P2;

    private void Update()
    {
        if (spellSlot_P1.spell == null) spellName_P1.text = "None";
        else spellName_P1.text = spellSlot_P1.spell.spellName;

        if (spellSlot_P2.spell == null) spellName_P2.text = "None";
        else spellName_P2.text = spellSlot_P2.spell.spellName;
    }
}
