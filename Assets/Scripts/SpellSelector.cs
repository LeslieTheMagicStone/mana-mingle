using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class SpellSelector : MonoBehaviour
{
    [SerializeField] private List<Transform> stopPoints;
    [SerializeField] private Transform selectPoint;
    [SerializeField] private Transform canvas;
    [SerializeField] private SpellPreview origSpellPreview;
    public List<SpellPreview> cards;
    private Vector3[] stopPositions;
    const float ANIM_TIME = 0.3f;
    private List<Sequence> sequences;

    private void Awake()
    {
        cards = new();

        stopPositions = stopPoints.Select(x => x.position).ToArray();
        sequences = new();
    }

    public void RollSpell(){

    }

    public void PeekSpells(){

    }

    public void HideSpells(){

    }

    public void AddSpellPreview(SpellBase spell)
    {
        var spellPreview = Instantiate(origSpellPreview);
        spellPreview.Init(spell);
        spellPreview.transform.SetParent(canvas, false);
        spellPreview.gameObject.SetActive(true);
        cards.Add(spellPreview);
    }
}
