using UnityEngine;

public class FloatingObject : MonoBehaviour
{

    private Vector3 origLocalPos;
    private float startTime;
    private const float AMPLITUDE = 0.1f;
    private const float TIME_FACTOR = 2f;
    private void Start()
    {
        startTime = Time.time;
        origLocalPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = origLocalPos + AMPLITUDE * Mathf.Sin((Time.time - startTime) * TIME_FACTOR) * Vector3.up;
    }
}
