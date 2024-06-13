using UnityEngine;
using System.Linq;
using Unity.Netcode;

public enum HighlightMode
{
    Auto,
    Cursor
}

public class PickerLogic : MonoBehaviour
{
    public SpellSlotManager spellSlots => _spellSlots;

    [SerializeField] private SpellSlotManager _spellSlots;
    [SerializeField] private HighlightMode highlightMode;

    private PickableObject currentHighlighted;
    private string pickButtonName;

    const float MAX_RANGE = 4f;

    private void Awake()
    {
        var player = GetComponent<PlayerLogic>();
        pickButtonName = "Pick";
    }

    private void Update()
    {
        UpdatePickables();
        if (Input.GetButtonDown(pickButtonName))
        {
            if (currentHighlighted != null)
                currentHighlighted.OnPick(this);
        }
    }

    private void UpdatePickables()
    {

        if (highlightMode == HighlightMode.Cursor)
        {
            PickableObject newHighlighted = null;
            // Screen center ray
            Ray ray = Camera.main.ViewportPointToRay(new(0.5f, 0.5f, 0));
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 0.1f);
            if (Physics.Raycast(ray, out RaycastHit hit, MAX_RANGE))
                hit.collider.TryGetComponent(out newHighlighted);
            if (newHighlighted == currentHighlighted) return;
            if (currentHighlighted != null) currentHighlighted.OnHighlightExit();
            currentHighlighted = newHighlighted;
            if (currentHighlighted != null) currentHighlighted.OnHighlightEnter();
            return;
        }

        if (currentHighlighted != null)
        {
            if (Vector3.Distance(transform.position, currentHighlighted.transform.position) > MAX_RANGE)
            {
                currentHighlighted.OnHighlightExit();
                currentHighlighted = null;
            }
        }

        var pickables = FindObjectsByType<PickableObject>(FindObjectsSortMode.None);
        pickables = pickables.Where(x => Vector3.Distance(transform.position, x.transform.position) < MAX_RANGE).ToArray();
        pickables = pickables.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();

        if (pickables.Length == 0) return;

        if (pickables[0] != currentHighlighted)
        {
            if (currentHighlighted != null) currentHighlighted.OnHighlightExit();
            currentHighlighted = pickables[0];
            pickables[0].OnHighlightEnter();
        }
    }
}
