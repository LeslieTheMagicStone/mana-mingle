using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    public string spellName { get; protected set; }
    public string displayName { get; protected set; }
    public int mana { get; protected set; }
    public GameObject appearance { get; protected set; }

    public abstract void Cast(PlayerLogic master);
}
