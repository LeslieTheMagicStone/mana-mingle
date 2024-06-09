using UnityEngine;
using System;

public class ModelPreviewManager : MonoBehaviour
{
    public static ModelPreviewManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SwitchVariant(PlayerVariant targetVariant)
    {
        foreach (PlayerVariant variant in Enum.GetValues(typeof(PlayerVariant)))
            transform.GetChild((int)variant).gameObject.SetActive(variant == targetVariant);
    }
}
