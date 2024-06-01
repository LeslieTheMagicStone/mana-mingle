using UnityEngine;
using TMPro;

public class SpellSlot : MonoBehaviour
{
    public SpellBase spell { get => _spell; set { _spell = value; nameText.text = value.displayName; } }
    private SpellBase _spell;
    private TMP_Text nameText;

    private void Awake()
    {
        nameText = GetComponentInChildren<TMP_Text>();
    }
}