using UnityEngine;

public class DustCellLogic : MonoBehaviour
{
    bool isEnabled = false;
    private void Update()
    {
        if (isEnabled) return;
        if (Vector3.Distance(DustLogic.instance.transform.position, transform.position) > DustLogic.instance.radius)
        {
            tag = "DustStorm";
            GetComponent<ParticleSystem>().Play();
            isEnabled = true;
        }
    }
}
