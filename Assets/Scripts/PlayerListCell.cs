using UnityEngine;
using TMPro;

public class PlayerListCell : MonoBehaviour
{
    private TMP_Text _id;
    private GameObject _isReady;
    private GameObject _notReady;
    private GameObject _lightVariant;
    private GameObject _darkVariant;
    public PlayerInfo playerInfo { get; private set; }

    public void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        _id = transform.Find("Id").GetComponent<TMP_Text>();
        _isReady = transform.Find("IsReady").gameObject;
        _notReady = transform.Find("NotReady").gameObject;
        _lightVariant = transform.Find("Variant/LightVariant").gameObject;
        _darkVariant = transform.Find("Variant/DarkVariant").gameObject;
        _id.text = playerInfo.id.ToString();
        _isReady.SetActive(playerInfo.isReady);
        _notReady.SetActive(!playerInfo.isReady);
        _lightVariant.SetActive(playerInfo.variant == PlayerVariant.Light);
        _darkVariant.SetActive(playerInfo.variant == PlayerVariant.Dark);
    }

    public void UpdateInfo(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        SetReady(playerInfo.isReady);
        SetVariant(playerInfo.variant);
    }

    private void SetReady(bool value)
    {
        _isReady.SetActive(value);
        _notReady.SetActive(!value);
    }

    private void SetVariant(PlayerVariant variant)
    {
        _lightVariant.SetActive(variant == PlayerVariant.Light);
        _darkVariant.SetActive(variant == PlayerVariant.Dark);
    }
}
