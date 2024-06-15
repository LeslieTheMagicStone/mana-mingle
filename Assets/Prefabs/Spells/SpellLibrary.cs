using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellLibrary", menuName = "SpellLibrary", order = 0)]
public class SpellLibrary : ScriptableObject
{
    public List<SpellBase> spells;
}