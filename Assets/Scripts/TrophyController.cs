using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrophyController : MonoBehaviour
{
    public float floatHeight = 0.5f;
    public float floatDuration = 2f;
    public float moveDuration = 5f;

    public static TrophyController instance { get; private set; }

    Tweener tweener;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 设置奖杯悬浮在空中
        FloatAnimation();
    }

    public void MoveToPlayer(Transform playerTransform)
    {
        // 停止悬浮动画
        DOTween.Kill(transform);

        // 计算目标位置
        Vector3 targetPosition = playerTransform.position + playerTransform.forward * 2f + Vector3.up * 1.5f;

        tweener.Kill();
        // 移动到玩家面前
        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutSine).OnComplete(FloatAnimation);
    }

    private void FloatAnimation()
    {
        // 设置奖杯悬浮在空中
        tweener = transform.DOMoveY(transform.position.y + floatHeight, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}

