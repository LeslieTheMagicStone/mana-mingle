using Unity.Netcode;
using UnityEngine;

public class PickableSpell : PickableObject
{
    public readonly NetworkVariable<SpellVariant> spellVariant = new(writePerm: NetworkVariableWritePermission.Server);
    public SpellBase spell { get; private set; }
    [SerializeField] private Transform display;
    private const float DISPLAY_SPEED = 90f;

    public override void OnNetworkSpawn()
    {
        Init();
    }

    public override void Init()
    {
        print("On value changed.");
        Transform appearance = display.GetChild((int)spellVariant.Value);
        appearance.gameObject.SetActive(true);
        targetRenderer = appearance.GetComponent<MeshRenderer>();
        spell = appearance.GetComponent<SpellBase>();
        base.Init();
    }

    private void Update()
    {
        display.Rotate(Vector3.up, DISPLAY_SPEED * Time.deltaTime);
    }

    public override void OnPick(PickerLogic picker)
    {
        picker.backpack.AddSpell(spell);
        Despawn();
    }
}
