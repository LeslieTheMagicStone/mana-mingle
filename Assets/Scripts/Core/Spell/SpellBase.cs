using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    public string spellName => _spellName;
    public string displayName => _displayName;
    public int mana => _mana;
    public GameObject prefab => _prefab;

    [SerializeField] private string _spellName;
    [SerializeField] private string _displayName;
    [SerializeField] private int _mana;
    [SerializeField] private GameObject _prefab;

    public abstract void Cast(PlayerLogic master);
}
