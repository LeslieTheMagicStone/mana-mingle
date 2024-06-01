using UnityEngine;

public class PickableSpell : PickableObject
{
    [SerializeField] private SpellBase spell;

    public override void OnPick(PickerLogic picker)
    {
        picker.spellSlots.AddSpell(spell);
        Destroy(gameObject);
    }
}
