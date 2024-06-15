using Unity.Netcode;
using UnityEngine;

public enum SpellVariant
{
    Incendio,
    Blizzard,
    Pilipala,
    AvadaKedavra,
    Crucio,
}

public enum SpellType
{
    Projectile,
    Stay,
}

public abstract class SpellBase : MonoBehaviour
{
    public SpellVariant spellVariant => _spellVariant;
    public string spellName => _spellName;
    public string displayName => _displayName;
    public int mana => _mana;
    public AudioClip audioClip => _audioClip;
    public SpellType spellType => _spellType;

    [SerializeField] private SpellVariant _spellVariant;
    [SerializeField] private string _spellName;
    [SerializeField] private string _displayName;
    [SerializeField] private int _mana;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool onlyBehaviour;
    [SerializeField] private SpellType _spellType;
}
