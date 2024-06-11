using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameLogic : NetworkBehaviour
{
    public static GameLogic instance { get; private set; }
    [SerializeField] private GameObject chat;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject origChatCell;
    [SerializeField] private Button sendButton;

    public override void OnNetworkSpawn()
    {
        instance = this;
        sendButton.onClick.AddListener(OnSendClick);
        input.onSubmit.AddListener((s) => OnSendClick());
        chat.SetActive(false);
    }

    public void SetChatActive(bool value) => chat.SetActive(value);

    public Transform GetSpawnPoint()
    {
        return GameObject.FindWithTag("SpawnPoint").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash)) { SetChatActive(true); input.ActivateInputField(); }
        if (Input.GetKeyDown(KeyCode.Escape)) { SetChatActive(false); }
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
