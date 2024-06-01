using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpellSlotManager : MonoBehaviour
{
    public SpellBase spell => currentSlot.spell;

    [SerializeField] private int maxSpells;
    [SerializeField] private SpellSlot spellSlotPrefab;
    [SerializeField] private PlayerLogic attachedPlayer;

    private SpellSlot[] spellSlots;
    private int emptySlotCount => spellSlots.Count(s => s.spell == null);
    private int currentSlotIndex;
    private SpellSlot currentSlot => spellSlots[currentSlotIndex];

    private void Awake()
    {
        spellSlots = new SpellSlot[maxSpells];
        for (int i = 0; i < maxSpells; i++)
        {
            var spellSlot = Instantiate(spellSlotPrefab, transform);
            spellSlots[i] = spellSlot;
            spellSlot.GetComponent<Button>().onClick.AddListener(() => SetSlot(spellSlot));
        }
        currentSlotIndex = 0;
        currentSlot.SetHighlight(true);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Prev" + attachedPlayer.playerID.ToString()))
        {
            currentSlot.SetHighlight(false);
            currentSlotIndex--;
            if (currentSlotIndex < 0) currentSlotIndex = spellSlots.Length - 1;
            currentSlot.SetHighlight(true);
        }
        if (Input.GetButtonDown("Next" + attachedPlayer.playerID.ToString()))
        {
            currentSlot.SetHighlight(false);
            currentSlotIndex++;
            if (currentSlotIndex >= spellSlots.Length) currentSlotIndex = 0;
            currentSlot.SetHighlight(true);
        }
    }

    public void AddSpell(SpellBase spell)
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

    private void SetSlot(SpellSlot slot)
    {
        for (int i = 0; i < maxSpells; i++)
        {
            if (spellSlots[i] == slot)
            {
                currentSlot.SetHighlight(false);
                currentSlotIndex = i;
                currentSlot.SetHighlight(true);
                return;
            }
        }
    }
}