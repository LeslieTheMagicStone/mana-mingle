using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpellSlotManager : MonoBehaviour
{
    public SpellBase spell => _spell;

    [SerializeField] private int maxSpells;
    [SerializeField] private SpellSlot spellSlotPrefab;

    private SpellBase _spell;
    private SpellSlot[] spellSlots;
    private int emptySlotCount => spellSlots.Count(s => s.spell == null);

    private void Awake()
    {
        spellSlots = new SpellSlot[maxSpells];
        for (int i = 0; i < maxSpells; i++)
        {
            var spellSlot = Instantiate(spellSlotPrefab, transform);
            spellSlots[i] = spellSlot;
            spellSlot.GetComponent<Button>().onClick.AddListener(() => SetSpell(spellSlot.spell));
        }
    }

    private void AddSpell(SpellBase spell)
    {
        if (spell == null) return;
        if (emptySlotCount == 0) return;
        for (int i = 0; i < maxSpells; i++)
        {
            if (spellSlots[i].spell == null)
            {
                spellSlots[i].spell = spell;
                break;
            }
        }
    }

    private void SetSpell(SpellBase spell)
    {
        if (spell == null) return;
        _spell = spell;
    }

}