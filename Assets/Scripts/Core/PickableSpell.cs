using UnityEngine;

public class PickableSpell : PickableObject
{
    [SerializeField] private SpellBase spell;
    [SerializeField] private Transform display;
    private const float DISPLAY_SPEED = 90f;

    protected override void Awake()
    {
        var appearance = Instantiate(spell.gameObject, display);
        targetRenderer = appearance.GetComponent<MeshRenderer>();
        base.Awake();
    }

    private void Update()
    {
        display.Rotate(Vector3.up, DISPLAY_SPEED * Time.deltaTime);
    }

    public override void OnPick(PickerLogic picker)
    {
        picker.backpack.AddSpell(spell);
        Destroy(gameObject);
    }
}
