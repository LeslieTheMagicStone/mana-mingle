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
}
