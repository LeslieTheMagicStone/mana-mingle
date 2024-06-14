using System.Collections.Generic;
using UnityEngine;

public class TestLogic : MonoBehaviour
{
    [SerializeField] private SpellSelector spellSelector;

    [SerializeField] private List<SpellBase> spells;

    private void Start()
    {
        foreach (var spell in spells) spellSelector.AddSpellPreview(spell);
    }

    private void Update()
    {
        if (Input.GetKeyDown("s")) spellSelector.StartSelecting();
        if (Input.GetKeyDown("k")) print(spellSelector.Select().displayName);
    }
}
