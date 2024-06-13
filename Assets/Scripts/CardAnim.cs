using UnityEngine;
using DG.Tweening;

public class CardAnim : MonoBehaviour
{
    private float enterDistance;
    private bool onMouseStay;
    private Vector3 origRot;
    private Tweener tweener;

    private void Awake()
    {
        onMouseStay = false;
        origRot = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (!onMouseStay) return;

        var localPos = Camera.main.WorldToScreenPoint(transform.position);
        var relativePos = Input.mousePosition - localPos;
        var axis = Vector3.Cross(relativePos, Vector3.forward);
        axis *= enterDistance / relativePos.magnitude / 8f;
        transform.localRotation = Quaternion.Euler(origRot + axis);
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
        tweener = transform.DOLocalRotate(origRot, 0.5f).SetEase(Ease.OutBounce);
    }

}
