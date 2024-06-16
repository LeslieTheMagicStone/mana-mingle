using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public int R;
    public GameObject Dust;
    float startTime;
    public static DustLogic instance { get; private set; }
    public float radius;

    private float GetRadius(float t)
    {
        return 30 + 300 * Mathf.Pow(2f, -t / 150f);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startTime = Time.time;

        while (R > 60)
        {
            for (float i = 0; i <= 360; i += 2000 / R)
            {
                Vector3 pos = new Vector3(R * Mathf.Cos(i), -30, R * Mathf.Sin(i));
                pos += transform.position;
                Instantiate(Dust, pos, Quaternion.identity);
            }
            R -= 20;
        }
    }

    private void Update()
    {
        radius = GetRadius(Time.time - startTime);
        transform.localScale = 2 * Vector3.one * radius;
    }
}
