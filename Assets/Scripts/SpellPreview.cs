using UnityEngine;

public enum CardStatus { Candidate, InHand, Used, InGraveyard }

public class SpellPreview : MonoBehaviour
{
    public SpellBase spell { get; private set; }
    public CardStatus cardStatus;
    public void Init(SpellVariant spellVariant)
    {
        spell = GameLogic.instance.spellLibrary[(int)spellVariant];
        transform.GetChild(0).GetComponent<MeshFilter>().mesh
            = spell.GetComponent<MeshFilter>().sharedMesh;
        transform.GetChild(0).GetComponent<MeshRenderer>().material
            = spell.GetComponent<MeshRenderer>().sharedMaterial;
        cardStatus = CardStatus.Candidate;
    }
    public void Refresh()
    {
        spell = null;
        transform.GetChild(0).GetComponent<MeshFilter>().mesh = null;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = null;
        cardStatus = CardStatus.Candidate;
    }

    public void SetTransparency(float alpha)
    {
        Renderer renderer = transform.GetChild(0).GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;

            if (alpha < 1.0f)
            {
                renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                renderer.material.SetInt("_ZWrite", 0);
                renderer.material.DisableKeyword("_ALPHATEST_ON");
                renderer.material.EnableKeyword("_ALPHABLEND_ON");
                renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                renderer.material.renderQueue = 3000;
            }
            else
            {
                renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                renderer.material.SetInt("_ZWrite", 1);
                renderer.material.DisableKeyword("_ALPHABLEND_ON");
                renderer.material.EnableKeyword("_ALPHATEST_ON");
                renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                renderer.material.renderQueue = -1;
            }
        }
    }
}
