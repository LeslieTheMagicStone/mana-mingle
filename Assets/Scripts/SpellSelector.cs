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
    public List<SpellPreview> spellPreviews;
    private Vector3[] stopPositions;
    const float ANIM_TIME = 0.5f;
    private Sequence sequence;

    private void Awake()
    {
        spellPreviews = new();

        stopPositions = stopPoints.Select(x => x.position).ToArray();
    }

    public void AddSpellPreview(SpellBase spell)
    {
        var spellPreview = Instantiate(origSpellPreview);
        spellPreview.Init(spell);
        spellPreview.transform.SetParent(canvas, false);
        spellPreview.gameObject.SetActive(true);
        spellPreviews.Add(spellPreview);
    }

    public void StartSelecting()
    {
        //test
        // spellPreviews[0].transform.DOPath(stopPositions, ANIM_TIME * stopPositions.Length);

        sequence?.Kill();
        sequence = DOTween.Sequence();

        for (int i = 0; i < spellPreviews.Count; i++)
        {
            sequence.AppendCallback(() => spellPreviews[i].transform.DOPath(stopPositions, ANIM_TIME * stopPositions.Length).SetDelay(0));
            sequence.AppendInterval(ANIM_TIME);
        }
    }

    public SpellBase Select()
    {
        SpellPreview closestSpell = null;
        float minDis = 0;

        for (int i = 0; i < spellPreviews.Count; i++)
        {
            float dis = Vector3.Distance(selectPoint.position, spellPreviews[i].transform.position);
            if (i == 0 || dis < minDis)
            {
                minDis = dis;
                closestSpell = spellPreviews[i];
            }
        }

        StopSelecting();

        return closestSpell.spell;
    }

    public void StopSelecting()
    {
        sequence?.Kill();
    }
}
