using UnityEngine;
using TMPro;

public class PlayerListCell : MonoBehaviour
{
    private TMP_Text _name;
    private TMP_Text _ready;
    public PlayerInfo playerInfo { get; private set; }

    public void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        _name = transform.Find("Name").GetComponent<TMP_Text>();
        _ready = transform.Find("Ready").GetComponent<TMP_Text>();
        _name.text = playerInfo.id.ToString();
        _ready.text = playerInfo.isReady ? "OK" : "X";
    }

    public void SetReady(bool value)
    {
        _ready.text = playerInfo.isReady ? "OK" : "X";
    }
}
