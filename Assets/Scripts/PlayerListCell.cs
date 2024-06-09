using UnityEngine;
using TMPro;

public class PlayerListCell : MonoBehaviour
{
    private TMP_Text _id;
    private GameObject _isReady;
    private GameObject _notReady;
    public PlayerInfo playerInfo { get; private set; }

    public void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        _id = transform.Find("Id").GetComponent<TMP_Text>();
        _isReady = transform.Find("IsReady").gameObject;
        _notReady = transform.Find("NotReady").gameObject;
        _id.text = playerInfo.id.ToString();
        _isReady.SetActive(playerInfo.isReady);
        _notReady.SetActive(!playerInfo.isReady);
    }

    public void SetReady(bool value)
    {
        _isReady.SetActive(value);
        _notReady.SetActive(!value);
    }
}
