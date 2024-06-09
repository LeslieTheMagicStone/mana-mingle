using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
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

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref isReady);
        serializer.SerializeValue(ref variant);
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

    private Dictionary<ulong, PlayerListCell> cells;
    private Dictionary<ulong, PlayerInfo> playerInfos;

    public override void OnNetworkSpawn()
    {
        if (IsServer) NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;

        startButton.onClick.AddListener(OnStartClick);
        readyToggle.onValueChanged.AddListener(OnReadyToggle);
        lightToggle.onValueChanged.AddListener(OnLightToggle);
        darkToggle.onValueChanged.AddListener(OnDarkToggle);
        cells = new();
        playerInfos = new();

        PlayerInfo playerInfo = new()
        {
            id = NetworkManager.LocalClientId,
            isReady = false,
            variant = PlayerVariant.Light,
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
            isReady = false
        };
        AddPlayer(playerInfo);
        UpdatePlayerInfos();
    }

    private void OnStartClick()
    {

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
        var info = playerInfos[NetworkManager.LocalClientId];
        info.variant = PlayerVariant.Light;
        playerInfos[NetworkManager.LocalClientId] = info;
        cells[NetworkManager.LocalClientId].UpdateInfo(info);
        if (IsServer) UpdatePlayerInfos();
        else UpdatePlayerInfosServerRpc(info);
        ModelPreviewManager.instance.SwitchVariant(PlayerVariant.Light);
    }

    private void OnDarkToggle(bool value)
    {
        if (!value) return;
        var info = playerInfos[NetworkManager.LocalClientId];
        info.variant = PlayerVariant.Dark;
        playerInfos[NetworkManager.LocalClientId] = info;
        cells[NetworkManager.LocalClientId].UpdateInfo(info);
        if (IsServer) UpdatePlayerInfos();
        else UpdatePlayerInfosServerRpc(info);
        ModelPreviewManager.instance.SwitchVariant(PlayerVariant.Dark);
    }

    private void UpdatePlayerInfo(ulong id, bool isReady)
    {
        PlayerInfo info = playerInfos[id];
        info.isReady = isReady;
        playerInfos[id] = info;
    }

    private void UpdatePlayerInfos()
    {
        foreach (var item in playerInfos)
            UpdatePlayerInfoClientRpc(item.Value);
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
