using UnityEngine;

public class SpellPreview : MonoBehaviour
{
    public SpellBase spell { get; private set; }
    public void Init(SpellBase spell)
    {
        GetComponent<MeshFilter>().mesh
            = spell.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshRenderer>().material
            = spell.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
