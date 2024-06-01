using UnityEngine;

public class TimerBar : MonoBehaviour
{
    private float origScaleX;

    private void Start()
    {
        origScaleX = transform.localScale.x;
    }

    private void Update()
    {
        if (GameManager.instance.stateTimer > 0)
        {
            var localScale = transform.localScale;
            localScale.x = GameManager.instance.stateTimer / GameManager.instance.stateTimerMax * origScaleX;
            transform.localScale = localScale;
        }
    }
}
