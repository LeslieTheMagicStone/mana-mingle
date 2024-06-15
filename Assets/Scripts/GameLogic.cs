using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;
using System;
using System.Linq;

public class GameLogic : NetworkBehaviour
{
    public static GameLogic instance { get; private set; }
    public GameObject localPlayer { get; private set; }
    [SerializeField] private Transform overlayCanvas;
    public List<SpellBase> spellLibrary => _spellLibrary;
    [SerializeField] private List<SpellBase> _spellLibrary;
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
    [SerializeField] private GameObject spellPreviewCanvas;
    [SerializeField] private SpellPreview origSpellPreview;
    [SerializeField] private RectTransform spellPreviewContent;
    private List<SpellPreview> deck;
    [Header("Spell Spawn")]
    [SerializeField] private NetworkObject pickableSpellPrefab;
    [SerializeField] private float minSpellIntensity;
    [SerializeField] private float maxSpellIntensity;
    [Header("Spells in Hand")]
    [SerializeField] private Transform spellsInHandContent;
    private int maxSpellsInHand => spellsInHandContent.childCount;

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

        deck = new();

        UiDepth = 0;
        SetCursorLock(true);

        if (IsServer) ServerSideInit();
    }

    // private void OnGUI()
    // {
    //     GUILayout.Label(Cursor.lockState.ToString());
    //     GUILayout.Label($"UIDepth: {UiDepth}");
    // }

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

        if (Input.GetKeyDown("r"))
        {
            Roll();
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
        if (spellPreviewCanvas.activeSelf == value) return;
        spellPreviewCanvas.SetActive(value);
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
        // print($"Set Cursor Lock State to {Cursor.lockState == CursorLockMode.None}");
        SetCursorLock(Cursor.lockState == CursorLockMode.None);
    }

    public void AddSpellPreview(SpellVariant spellVariant)
    {
        var spellPreview = Instantiate(origSpellPreview);
        spellPreview.transform.SetParent(spellPreviewContent, false);
        spellPreview.Init(spellVariant);
        spellPreview.gameObject.SetActive(true);
        deck.Add(spellPreview);
    }

    public Transform GetSpawnPoint()
    {
        return GameObject.FindWithTag("SpawnPoint").transform;
    }

    private void ServerSideInit()
    {
        // Init pickable spells
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpellSpawnPoint");
        int minRequirement = (int)(spawnPoints.Length * minSpellIntensity);
        int maxRequirement = (int)(spawnPoints.Length * maxSpellIntensity);
        foreach (var spawnPoint in spawnPoints)
        {
            if (maxRequirement <= 0) break;
            if (minRequirement <= 0 && UnityEngine.Random.value < 0.5f) continue;
            minRequirement--;
            maxRequirement--;

            var clone = Instantiate(pickableSpellPrefab);
            clone.transform.position = spawnPoint.transform.position;
            var pickableSpell = clone.GetComponent<PickableSpell>();
            var spellVariant = UnityEngine.Random.Range(0, Enum.GetValues(typeof(SpellVariant)).Length);
            pickableSpell.spellVariant.Value = (SpellVariant)spellVariant;
            clone.Spawn();
        }
    }

    private void Roll()
    {
        deck[0].cardStatus = CardStatus.InHand;
        spellsInHandContent.GetChild(0).GetComponent<SpellPreview>().Init(deck[0].spell.spellVariant);
        // var candidates = deck.Where(x => x.cardStatus == CardStatus.Candidate).ToList();
        // for (int i = 0; i < maxSpellsInHand; i++)
        // {
        //     if (candidates.Count == 0)
        //     {
        //         if (deck.Count <= maxSpellsInHand) { print("卡不足"); break; };

        //         print("洗牌");
        //         candidates = deck.Where(x => x.cardStatus == CardStatus.Used).ToList();
        //         foreach (var candidate in candidates)
        //             candidate.cardStatus = CardStatus.Candidate;
        //         candidates = deck.Where(x => x.cardStatus == CardStatus.Candidate).ToList();
        //     }
        //     var index = UnityEngine.Random.Range(0, candidates.Count);
        //     candidates[index].cardStatus = CardStatus.InHand;
        //     spellsInHandContent.GetChild(i).GetComponent<SpellPreview>().Init(candidates[index]);
        //     candidates.RemoveAt(index);
        // }
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
