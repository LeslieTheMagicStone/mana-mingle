using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSFXLogic : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private AudioClip OnFocusSFX;
    [SerializeField] private AudioClip OnClickSFX;
    private Button button;
    private AudioSource audioSource;

    private void Awake()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickSFX == null) return;
        audioSource.clip = OnClickSFX;
        audioSource.Play();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (OnFocusSFX == null) return;
        audioSource.clip = OnFocusSFX;
        audioSource.Play();
    }
}

