using TMPro;
using UnityEngine;

public class LocData : MonoBehaviour
{
    [SerializeField] private string locKey;

    private void Start()
    {
        if (!TryGetComponent(out TextMeshProUGUI tmp)) return;
        tmp.text = LocalizationManager.Instance.GetLocString(locKey);
        LocalizationManager.Instance.OnLanguageChanged.AddListener(
            () => tmp.text = LocalizationManager.Instance.GetLocString(locKey));
    }
}
