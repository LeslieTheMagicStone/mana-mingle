using UnityEngine;

public class SpellPreview : MonoBehaviour
{
    public SpellBase spell { get; private set; }
    public void Init(SpellBase spell)
    {
        transform.GetChild(0).GetComponent<MeshFilter>().mesh
            = spell.GetComponent<MeshFilter>().sharedMesh;
        transform.GetChild(0).GetComponent<MeshRenderer>().material
            = spell.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
