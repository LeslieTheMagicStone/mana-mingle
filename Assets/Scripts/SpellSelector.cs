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
    const float ANIM_TIME = 1f;
    private List<Sequence> sequences;

    private void Awake()
    {
        cards = new();

        stopPositions = stopPoints.Select(x => x.position).ToArray();
        sequences = new();
    }

    public void AddSpellPreview(SpellBase spell)
    {
        var spellPreview = Instantiate(origSpellPreview);
        spellPreview.Init(spell);
        spellPreview.transform.SetParent(canvas, false);
        spellPreview.gameObject.SetActive(true);
        cards.Add(spellPreview);
    }

    public void StartSelecting()
    {
        KillSequences();
        float delayBetweenCards = ANIM_TIME / cards.Count;

        for (int i = 0; i < cards.Count; i++)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(cards[i].transform.DOPath(stopPositions, ANIM_TIME, PathType.Linear).SetEase(Ease.Linear));
            sequence.SetDelay(delayBetweenCards * i);
            sequence.SetLoops(-1, LoopType.Restart);

            sequences.Add(sequence);
        }
    }

    public SpellBase Select()
    {
        SpellPreview closestSpell = null;
        float minDis = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            float dis = Vector3.Distance(selectPoint.position, cards[i].transform.position);
            if (i == 0 || dis < minDis)
            {
                minDis = dis;
                closestSpell = cards[i];
            }
        }

        StopSelecting();

        return closestSpell.spell;
    }

    public void StopSelecting()
    {
        KillSequences();
    }

    private void KillSequences()
    {
        foreach (var sequence in sequences) sequence.Kill();
        sequences = new();
    }
}
