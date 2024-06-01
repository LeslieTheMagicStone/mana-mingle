using UnityEngine;
using UnityEngine.EventSystems;

public class WandLogic : MonoBehaviour
{
    [SerializeField] private SpellSlotManager spellSlot;
    [SerializeField] private ManaLogic manaLogic;
    private Animator animator;
    private PlayerLogic playerLogic;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerLogic = GetComponentInParent<PlayerLogic>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.currentSelectedGameObject != null) return;
            if (spellSlot.spell == null) return;
            if (!manaLogic.TryCostMana(spellSlot.spell.mana)) { print("Not enough mana."); return; }
            spellSlot.spell.Cast(playerLogic);
            animator.SetTrigger("Cast");
        }
    }
}
