using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameLogic : NetworkBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject origChatCell;
    [SerializeField] private Button sendButton;

    public override void OnNetworkSpawn()
    {
        sendButton.onClick.AddListener(OnSendClick);
    }

    private void OnSendClick()
    {
        if (string.IsNullOrEmpty(input.text)) return;

        PlayerInfo playerInfo = GameManager.instance.playerInfos[NetworkManager.LocalClientId];
        AddChatCell(playerInfo.name, input.text);

        if (IsServer) SendMsgToOthersClientRpc(playerInfo, input.text);
        else SendMsgToOthersServerRpc(playerInfo, input.text);
    }

    [ClientRpc]
    void SendMsgToOthersClientRpc(PlayerInfo playerInfo, string content)
    {
        if (IsHost || playerInfo.id == NetworkManager.LocalClientId) return;
        AddChatCell(playerInfo.name, content);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendMsgToOthersServerRpc(PlayerInfo playerInfo, string content)
    {
        AddChatCell(playerInfo.name, content);
        SendMsgToOthersClientRpc(playerInfo, content);
    }

    private void AddChatCell(string playerName, string content)
    {
        GameObject clone = Instantiate(origChatCell);
        clone.transform.SetParent(this.content, false);
        clone.AddComponent<ChatCell>().Init(playerName, content);
        clone.SetActive(true);
    }
}
