using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrophyRotate : MonoBehaviour
{
    public GameObject trophy; // 拖动奖杯对象到此处

    void Start()
    {
        // 设置奖杯悬浮在空中
        trophy.transform.DOMoveY(trophy.transform.position.y + 0.5f, 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        // 设置空对象绕Y轴旋转的动画
        transform.DORotate(new Vector3(0, 360, 0), 10f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}
