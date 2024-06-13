using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;
using Sun_Temple;

public class GameLogic : NetworkBehaviour
{
    public static GameLogic instance { get; private set; }
    public GameObject localPlayer { get; private set; }
    [SerializeField] private Transform overlayCanvas;
    private int UiDepth;
    private int isCursorLocked;
    [Header("Chat")]
    [SerializeField] private GameObject chat;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private RectTransform chatContent;
    [SerializeField] private GameObject origChatCell;
    [SerializeField] private Button sendButton;
    [Header("Player Info")]
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Transform models;
    [Header("Card Shower")]
    [SerializeField] private GameObject cardPreviewPanel;
    [SerializeField] private GameObject origCardPreviewCell;
    [SerializeField] private RectTransform cardPreviewContent;
    private List<GameObject> cardPreviewCells;
    [SerializeField] private SpellBase testspell;

    public override void OnNetworkSpawn()
    {
        instance = this;
        sendButton.onClick.AddListener(OnSendClick);
        input.onSubmit.AddListener((s) => OnSendClick());
        chat.SetActive(false);

        PlayerInfo playerInfo = GameManager.instance.playerInfos[NetworkManager.LocalClientId];
        playerName.text = playerInfo.name;

        localPlayer = NetworkManager.LocalClient.PlayerObject.transform.GetChild((int)playerInfo.variant).gameObject;
        var damageable = localPlayer.GetComponent<Damageable>();
        damageable._health.OnValueChanged += (prev, curr) => healthBar.value = (float)curr / damageable.maxHealth;
        print((int)playerInfo.variant);
        print(models.GetChild((int)playerInfo.variant).gameObject.name);
        models.GetChild((int)playerInfo.variant).gameObject.SetActive(true);

        cardPreviewCells = new();

        UiDepth = 0;
        SetCursorLock(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            SetChatActive(true);
            input.ActivateInputField();
        }

        if (Input.GetButtonDown("Backpack"))
            SetBackpackActive(true);
        if (Input.GetButtonUp("Backpack"))
            SetBackpackActive(false);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UiDepth == 0) SwitchCursorLock();
            SetChatActive(false);
            SetBackpackActive(false);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            print("ADD spell");
            AddSpellPreview(testspell);
        }
    }

    public void SetChatActive(bool value)
    {
        if (chat.activeSelf == value) return;
        chat.SetActive(value);
        UiDepth += value ? 1 : -1;
    }

    public void SetBackpackActive(bool value)
    {
        if (cardPreviewPanel.activeSelf == value) return;
        cardPreviewPanel.SetActive(value);
        overlayCanvas.gameObject.SetActive(!value);
        SetCursorLock(!value);
        UiDepth += value ? 1 : -1;
    }

    public void SetCursorLock(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SwitchCursorLock()
    {
        SetCursorLock(Cursor.lockState == CursorLockMode.None);
    }

    public void AddSpellPreview(SpellBase spell)
    {
        var card = Instantiate(origCardPreviewCell);
        card.transform.SetParent(cardPreviewContent, false);
        card.GetComponentInChildren<MeshFilter>().mesh =
            spell.GetComponent<MeshFilter>().sharedMesh;
        card.GetComponentInChildren<MeshRenderer>().material =
            spell.GetComponent<MeshRenderer>().sharedMaterial;
        card.SetActive(true);
        cardPreviewCells.Add(card);
    }

    public Transform GetSpawnPoint()
    {
        return GameObject.FindWithTag("SpawnPoint").transform;
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
        clone.transform.SetParent(this.chatContent, false);
        clone.AddComponent<ChatCell>().Init(playerName, content);
        clone.SetActive(true);
    }
}
