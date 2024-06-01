using UnityEngine;
using UnityEngine.Events;

public class ManaLogic : MonoBehaviour
{
    public UnityEvent onManaChanged;

    public int mana => _mana;
    public int maxMana => _maxMana;
    [SerializeField] private int _maxMana;
    private int _mana;

    private void Awake()
    {
        _mana = _maxMana;
    }

    public bool TryCostMana(int mana)
    {
        if (_mana >= mana)
        {
            _mana -= mana;
            onManaChanged.Invoke();
            return true;
        }
        return false;
    }
}
