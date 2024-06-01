using UnityEngine;
using TMPro;

public class SpellSlot : MonoBehaviour
{
    public SpellBase spell { get => _spell; set { _spell = value; nameText.text = value.displayName; } }
    [SerializeField] private GameObject highlight;
    private SpellBase _spell;
    private TMP_Text nameText;
    private bool isHighlighted;

    private void Awake()
    {
        nameText = GetComponentInChildren<TMP_Text>();
    }

    public void SetHighlight(bool value)
    {
        isHighlighted = value;
        highlight.SetActive(value);
    }
}