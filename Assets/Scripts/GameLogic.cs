using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameLogic : NetworkBehaviour
{
    public static GameLogic instance { get; private set; }
    public NetworkObject localPlayer { get; private set; }
    [Header("Chat")]
    [SerializeField] private GameObject chat;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject origChatCell;
    [SerializeField] private Button sendButton;
    [Header("Player Info")]
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Transform models;

    public override void OnNetworkSpawn()
    {
        instance = this;
        sendButton.onClick.AddListener(OnSendClick);
        input.onSubmit.AddListener((s) => OnSendClick());
        chat.SetActive(false);

        PlayerInfo playerInfo = GameManager.instance.playerInfos[NetworkManager.LocalClientId];
        playerName.text = playerInfo.name;
        
        localPlayer = NetworkManager.LocalClient.PlayerObject;
        var damageable = localPlayer.GetComponent<Damageable>();
        damageable._health.OnValueChanged += (prev, curr) => healthBar.value = (float)curr / damageable.maxHealth;
        models.GetChild((int)playerInfo.variant).gameObject.SetActive(true);
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
