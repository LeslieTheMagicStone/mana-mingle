using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerVariant
{
    Light,
    Dark,
}

public struct PlayerInfo : INetworkSerializable
{
    public ulong id;
    public bool isReady;
    public PlayerVariant variant;
    public string name;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref isReady);
        serializer.SerializeValue(ref variant);
        serializer.SerializeValue(ref name);
    }
}

public class LobbyLogic : NetworkBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject origCell;
    [SerializeField] private Button startButton;
    [SerializeField] private Toggle readyToggle;
    [SerializeField] private Toggle lightToggle;
    [SerializeField] private Toggle darkToggle;
    [SerializeField] private TMP_InputField nameInput;

    private Dictionary<ulong, PlayerListCell> cells;
    private Dictionary<ulong, PlayerInfo> playerInfos;

    public override void OnNetworkSpawn()
    {
        if (IsServer) NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;

        startButton.onClick.AddListener(OnStartClick);
        readyToggle.onValueChanged.AddListener(OnReadyToggle);
        lightToggle.onValueChanged.AddListener(OnLightToggle);
        darkToggle.onValueChanged.AddListener(OnDarkToggle);
        nameInput.onEndEdit.AddListener(OnNameEndEdit);
        cells = new();
        playerInfos = new();

        PlayerInfo playerInfo = new()
        {
            id = NetworkManager.LocalClientId,
            isReady = false,
            variant = PlayerVariant.Light,
            name = "Player " + NetworkManager.LocalClientId.ToString(),
        };
        AddPlayer(playerInfo);
    }

    public void AddPlayer(PlayerInfo playerInfo)
    {
        playerInfos.Add(playerInfo.id, playerInfo);

        GameObject clone = Instantiate(origCell);
        clone.transform.SetParent(content, false);

        var cell = clone.GetComponent<PlayerListCell>();
        cells.Add(playerInfo.id, cell);
        cell.Init(playerInfo);
        clone.SetActive(true);
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        PlayerInfo playerInfo = new()
        {
            id = clientId,
            isReady = false,
            variant = PlayerVariant.Light,
            name = "Player" + clientId.ToString(),
        };
        AddPlayer(playerInfo);
        UpdatePlayerInfos();
    }

    private void OnStartClick()
    {
        UploadPlayerInfosClientRpc();
        SceneController.instance.LoadScene(Scenes.GameScene);
    }

    private void OnNameEndEdit(string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        PlayerInfo playerInfo = playerInfos[NetworkManager.LocalClientId];
        playerInfo.name = value;
        playerInfos[NetworkManager.LocalClientId] = playerInfo;
        cells[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
        if (IsServer) UpdatePlayerInfos();
        else UpdatePlayerInfosServerRpc(playerInfo);
    }

    private void OnReadyToggle(bool value)
    {
        UpdatePlayerInfo(NetworkManager.LocalClientId, value);
        cells[NetworkManager.LocalClientId].UpdateInfo(playerInfos[NetworkManager.LocalClientId]);

        if (IsServer) UpdatePlayerInfos();
        else UpdatePlayerInfosServerRpc(playerInfos[NetworkManager.LocalClientId]);

    }

    private void OnLightToggle(bool value)
    {
        if (!value) return;
        var playerInfo = playerInfos[NetworkManager.LocalClientId];
        playerInfo.variant = PlayerVariant.Light;
        playerInfos[NetworkManager.LocalClientId] = playerInfo;
        cells[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
        if (IsServer) UpdatePlayerInfos();
        else UpdatePlayerInfosServerRpc(playerInfo);
        ModelPreviewManager.instance.SwitchVariant(PlayerVariant.Light);
    }

    private void OnDarkToggle(bool value)
    {
        if (!value) return;
        var playerInfo = playerInfos[NetworkManager.LocalClientId];
        playerInfo.variant = PlayerVariant.Dark;
        playerInfos[NetworkManager.LocalClientId] = playerInfo;
        cells[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
        if (IsServer) UpdatePlayerInfos();
        else UpdatePlayerInfosServerRpc(playerInfo);
        ModelPreviewManager.instance.SwitchVariant(PlayerVariant.Dark);
    }


    [ClientRpc]
    private void UploadPlayerInfosClientRpc() { GameManager.instance.StartGame(playerInfos); }

    private void UpdatePlayerInfo(ulong id, bool isReady)
    {
        PlayerInfo playerInfo = playerInfos[id];
        playerInfo.isReady = isReady;
        playerInfos[id] = playerInfo;
    }

    private void UpdatePlayerInfos()
    {
        bool canStart = true;
        foreach (var item in playerInfos)
        {
            if (!item.Value.isReady) canStart = false;
            UpdatePlayerInfoClientRpc(item.Value);
        }
        startButton.gameObject.SetActive(canStart);
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdatePlayerInfosServerRpc(PlayerInfo playerInfo)
    {
        playerInfos[playerInfo.id] = playerInfo;
        cells[playerInfo.id].UpdateInfo(playerInfo);
        UpdatePlayerInfos();
    }

    [ClientRpc]
    private void UpdatePlayerInfoClientRpc(PlayerInfo playerInfo)
    {
        if (IsHost) return;
        if (playerInfos.ContainsKey(playerInfo.id)) playerInfos[playerInfo.id] = playerInfo;
        else AddPlayer(playerInfo);
        UpdatePlayerCells();
    }

    private void UpdatePlayerCells()
    {
        foreach (var item in playerInfos)
        {
            cells[item.Key].UpdateInfo(item.Value);
        }
    }
}
