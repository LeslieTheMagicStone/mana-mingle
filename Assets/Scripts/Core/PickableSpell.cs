using UnityEngine;

public class PickableSpell : MonoBehaviour
{
    [SerializeField] private SpellBase spell;

    public void Pick(SpellSlotManager slots)
    {
        slots.AddSpell(spell);
    }

    private void OnMouseDown()
    {
        Pick(FindObjectOfType<SpellSlotManager>());
        Destroy(gameObject);
    }
}
