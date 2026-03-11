using UnityEngine;
using UnityEngine.Events;

public class ManaLogic : MonoBehaviour
{
    public UnityEvent onManaChanged;

    public int mana => _mana;
    public int maxMana => _maxMana;
    [SerializeField] private int _maxMana;
    [SerializeField] private int manaPerHalf;
    private int _mana;
    private float manaTimer;

    private void Awake()
    {
        _mana = _maxMana;
    }

    private void Update()
    {
        if (manaTimer > 0)
        {
            manaTimer -= Time.deltaTime;
        }
        else
        {
            _mana += manaPerHalf;
            if (_mana > _maxMana) _mana = _maxMana;
            onManaChanged.Invoke();
            manaTimer = 0.5f;
        }
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
