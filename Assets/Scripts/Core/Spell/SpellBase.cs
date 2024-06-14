using UnityEngine;

public enum SpellVariant
{
    Incendio,
    Blizzard,
    Pilipala,
}

public abstract class SpellBase : MonoBehaviour
{
    public SpellVariant spellVariant => _spellVariant;
    public string spellName => _spellName;
    public string displayName => _displayName;
    public int mana => _mana;

    [SerializeField] private SpellVariant _spellVariant;
    [SerializeField] private string _spellName;
    [SerializeField] private string _displayName;
    [SerializeField] private int _mana;

    public abstract void Cast(PlayerLogic master);
}
