using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public int R;
    public GameObject Dust;
    int ticks = 0;

    // Update is called once per frame
    void Update()
    {
        ticks++;
        if (ticks > 600 && R > 60)
        {
            ticks = 0;
            for (float i = 0; i <= 360; i += 2000 / R)
            {
                Vector3 pos = new Vector3(R * Mathf.Cos(i), -30, R * Mathf.Sin(i));
                pos += transform.position;
                Instantiate(Dust, pos, Quaternion.identity);
            }
            R -= 20;
        }
    }
}
