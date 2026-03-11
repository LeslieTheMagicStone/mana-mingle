using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DustLogic : NetworkBehaviour
{
    // Start is called before the first frame update
    public int R;
    public GameObject Dust;
    NetworkVariable<float> startTime = new(writePerm: NetworkVariableWritePermission.Server);
    public static DustLogic instance { get; private set; }
    public float radius;

    private float GetRadius(float t)
    {
        return 300 * Mathf.Pow(2f, -(t - 60) / 150f);
    }

    private void Awake()
    {
        instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            startTime.Value = Time.time;
        }
    }

    private void Start()
    {
        while (R > 60)
        {
            for (float i = 0; i <= 360; i += 2000 / R)
            {
                Vector3 pos = new Vector3(R * Mathf.Cos(i), -30, R * Mathf.Sin(i));
                pos += transform.position;
                var rot = Quaternion.LookRotation(transform.position - pos);
                Instantiate(Dust, pos, rot);
            }
            R -= 20;
        }
    }

    private void Update()
    {
        radius = GetRadius(Time.time - startTime.Value);
        transform.localScale = 2 * Vector3.one * radius;
    }
}
