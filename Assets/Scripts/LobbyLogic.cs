using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerInfo : INetworkSerializable
{
    public ulong id;
    public bool isReady;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref isReady);
    }
}

public class LobbyLogic : NetworkBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject origCell;
    [SerializeField] private Button startButton;
    [SerializeField] private Toggle readyToggle;

    private Dictionary<ulong, PlayerListCell> cells;
    private Dictionary<ulong, PlayerInfo> playerInfos;

    public override void OnNetworkSpawn()
    {
        if (IsServer) NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;

        startButton.onClick.AddListener(OnStartClick);
        readyToggle.onValueChanged.AddListener(OnReadyToggle);
        cells = new();
        playerInfos = new();

        PlayerInfo playerInfo = new()
        {
            id = NetworkManager.LocalClientId,
            isReady = false
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
        cells[NetworkManager.LocalClientId].SetReady(value);
        UpdatePlayerInfo(NetworkManager.LocalClientId, value);

        if (IsServer) UpdatePlayerInfos();
        else UpdatePlayerInfosServerRpc(playerInfos[NetworkManager.LocalClientId]);

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
        cells[playerInfo.id].SetReady(playerInfo.isReady);
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
            cells[item.Key].SetReady(item.Value.isReady);
        }
    }
}
