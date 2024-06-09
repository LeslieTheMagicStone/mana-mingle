using UnityEngine;
using TMPro;

public class PlayerListCell : MonoBehaviour
{
    private TMP_Text _name;
    private GameObject _isReady;
    private GameObject _notReady;
    private GameObject _lightVariant;
    private GameObject _darkVariant;
    public PlayerInfo playerInfo { get; private set; }

    public void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        _name = transform.Find("Name").GetComponent<TMP_Text>();
        _isReady = transform.Find("IsReady").gameObject;
        _notReady = transform.Find("NotReady").gameObject;
        _lightVariant = transform.Find("Variant/LightVariant").gameObject;
        _darkVariant = transform.Find("Variant/DarkVariant").gameObject;
        _name.text = playerInfo.name;
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
        SetName(playerInfo.name);
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

    private void SetName(string value)
    {
        _name.text = value;
    }
}
