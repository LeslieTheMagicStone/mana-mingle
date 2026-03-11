using UnityEngine;
using System.Linq;
using Unity.Netcode;

public enum HighlightMode
{
    Cursor,
    Auto,
}

public class PickerLogic : MonoBehaviour
{
    public Backpack backpack { get; private set; }

    [SerializeField] private HighlightMode highlightMode;

    private PickableObject currentHighlighted;

    const float MAX_RANGE = 4f;

    private void Awake()
    {
        backpack = GetComponent<Backpack>();
    }

    private void Update()
    {
        UpdatePickables();
        if (Input.GetButtonDown("Interact"))
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
