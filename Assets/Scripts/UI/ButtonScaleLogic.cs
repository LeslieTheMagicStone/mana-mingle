using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonScaleLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Tweener tweener;
    private Vector3 origScale;
    private const float ANIM_TIME = 0.1f;

    private void Awake()
    {
        origScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        tweener?.Kill();
        tweener = transform.DOScale(new Vector3(origScale.x * 1.1f, origScale.y * 1.1f, origScale.z), ANIM_TIME);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        tweener?.Kill();
        tweener = transform.DOScale(origScale, ANIM_TIME);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        tweener?.Kill();
        tweener = transform.DOScale(new Vector3(origScale.x * 0.95f, origScale.y * 0.95f, origScale.z), ANIM_TIME / 2);
        tweener.SetLoops(2, LoopType.Yoyo);
    }

    private void OnDestroy() {
        tweener?.Kill();
    }
}
