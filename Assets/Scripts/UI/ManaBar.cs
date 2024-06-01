using UnityEngine;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private ManaLogic master;
    private Vector3 localScale;
    private float maxWidth;

    private void Awake()
    {
        master.onManaChanged.AddListener(UpdateMana);

        maxWidth = transform.localScale.x;
        localScale = transform.localScale;
    }

    private void Start()
    {
        UpdateMana();
    }

    private void UpdateMana()
    {
        if (master == null) return;
        localScale.x = (float)master.mana / master.maxMana * maxWidth;
        transform.localScale = localScale;
    }

    private void OnGUI()
    {
        GUILayout.Label("Mana: " + master.mana);
    }
}
