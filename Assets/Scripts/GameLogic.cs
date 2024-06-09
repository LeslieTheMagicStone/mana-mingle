using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject origChatCell;
    [SerializeField] private Button sendButton;

    private void Awake() {
        sendButton.onClick.AddListener(OnSendClick);
    }

    private void OnSendClick(){
        if(string.IsNullOrEmpty(input.text)) return;

        PlayerInfo playerInfo  = GameManager.instance.playerInfos[NetworkManager.Singleton.LocalClientId];
        AddChatCell(playerInfo.name, input.text);
    }

    private void AddChatCell(string playerName, string content){
        GameObject clone = Instantiate(origChatCell);
        clone.transform.SetParent(this.content, false);
        clone.AddComponent<ChatCell>().Init(playerName, content);
        clone.SetActive(true);
    }
}
