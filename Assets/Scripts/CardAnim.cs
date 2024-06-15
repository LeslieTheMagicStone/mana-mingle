using UnityEngine;
using DG.Tweening;

public class CardAnim : MonoBehaviour
{
    private float enterDistance;
    private bool onMouseStay;
    private Quaternion origRot;
    private Quaternion targetRot;
    private Tweener tweener;
    private const float SMOOTH_FACTOR = 10f;

    private void Awake()
    {
        onMouseStay = false;
        origRot = transform.localRotation;
    }

    private void Update()
    {
        if (!onMouseStay) return;

        var localPos = Camera.main.WorldToScreenPoint(transform.position);
        var relativePos = Input.mousePosition - localPos;
        var axis = Vector3.Cross(relativePos, Vector3.forward);
        axis *= enterDistance / relativePos.magnitude / 4f;
        targetRot = Quaternion.Euler(axis) * origRot;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * SMOOTH_FACTOR);
    }

    private void OnMouseEnter()
    {
        var localPos = Camera.main.WorldToScreenPoint(transform.position);
        enterDistance = (Input.mousePosition - localPos).magnitude;
        onMouseStay = true;
        tweener?.Kill();
    }

    private void OnMouseExit()
    {
        enterDistance = 0f;
        onMouseStay = false;

        tweener?.Kill();
        tweener = transform.DOLocalRotate(origRot.eulerAngles, 0.5f).SetEase(Ease.OutBounce);
    }

}
