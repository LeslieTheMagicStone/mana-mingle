using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;
using System;
using System.Linq;
using DG.Tweening;
using SunTemple;

public class GameLogic : NetworkBehaviour
{
    public static GameLogic instance { get; private set; }
    public GameObject localPlayer { get; private set; }
    [SerializeField] private Transform overlayCanvas;
    public List<SpellBase> spellLibrary => _spellLibrary.spells;
    [SerializeField] private SpellLibrary _spellLibrary;
    private int UiDepth;
    private int isCursorLocked;
    public List<Damageable> players = new(); // Added in PlayerInit
    private TrophyController trophyController => TrophyController.instance;
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
    private bool isRolling;
    private Sequence rollSeq;
    private List<SpellPreview> spellsInHand;
    [Header("Player Death")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Button quit;
    [SerializeField] private Button watch;
    [SerializeField] private int watchPlayerIndex = -1;
    [SerializeField] private GameObject watchButtons;
    [SerializeField] private Button next;
    [SerializeField] private Button prev;
    private Damageable watchingPlayer;
    [Header("Player Win")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button winQuit;

    private int serverrpccalledtime = 0;
    private int clientrpccalledtime = 0;
    private bool trophyMoved = false;
    private bool isWinner = false;
    private bool localPlayerDead = false;
    private int playerCount = 3;

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
        var manaLogic = localPlayer.GetComponent<ManaLogic>();
        manaLogic.onManaChanged.AddListener(() => manaBar.value = (float)manaLogic.mana / manaLogic.maxMana);
        // print((int)playerInfo.variant);
        // print(models.GetChild((int)playerInfo.variant).gameObject.name);
        models.GetChild((int)playerInfo.variant).gameObject.SetActive(true);

        deck = new();

        UiDepth = 0;
        SetCursorLock(true);

        isRolling = false;
        spellsInHand = new();
        foreach (var spellPreview in spellsInHandContent.GetComponentsInChildren<SpellPreview>())
            spellPreview.Refresh();

        quit.onClick.AddListener(OnQuitClick);
        watch.onClick.AddListener(OnWatchClick);
        next.onClick.AddListener(OnNextClick);
        prev.onClick.AddListener(OnPrevClick);

        winQuit.onClick.AddListener(OnQuitClick);

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

        if (UiDepth == 0)
        {
            if (Input.GetButtonDown("Roll"))
            {
                Roll();
            }

            for (int i = 0; i < maxSpellsInHand; i++)
            {
                if (!Input.GetButtonDown("Fire" + (i + 1).ToString())) continue;
                if (spellsInHand.Count <= i) continue;
                var variant = spellsInHand[i].spell.spellVariant;
                localPlayer.GetComponent<PlayerLogic>().Cast(variant);
            }
        }

        if (watchPlayerIndex == -1) return;
        if (watchingPlayer.isDead)
            OnNextClick();
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

    [ServerRpc(RequireOwnership = false)]
    private void OnDeathServerRpc()
    {
        serverrpccalledtime++;
        playerCount--;
        if (playerCount == 1)
        {
            var alivePlayers = players.Where(x => x != null && !x.isDead).ToList();
            ulong winnerId = alivePlayers[0].OwnerClientId;
            Vector3 pos = alivePlayers[0].transform.position;
            Quaternion rot = alivePlayers[0].transform.rotation;
            OnVictoryClientRpc(winnerId, pos, rot);
        }
    }

    [ClientRpc]
    private void OnVictoryClientRpc(ulong winnerId, Vector3 pos, Quaternion rot)
    {
        print("Game over client rpc");
        clientrpccalledtime++;

        // 调用奖杯移动到玩家面前
        trophyController.MoveToPlayer(pos, rot);
        trophyMoved = true;

        if (winnerId == NetworkManager.LocalClientId)
        {
            isWinner = true;
            winPanel.SetActive(true);
            CursorManager.instance.gameObject.SetActive(false);
        }
    }


    public void OnPlayerDeath()
    {
        localPlayerDead = true;

        SetCursorLock(false);
        overlayCanvas.gameObject.SetActive(false);

        deathPanel.SetActive(true);
        CursorManager.instance.gameObject.SetActive(false);

        OnDeathServerRpc();
    }

    // private void OnGUI()
    // {
    //     var alivePlayers = players.Where(x => x != null && !x.isDead).ToList();
    //     GUILayout.Label("AlivePlayers: " + alivePlayers.Count.ToString());
    //     GUILayout.Label("");
    //     GUILayout.Label("");
    //     GUILayout.Label("");
    //     GUILayout.Label("");
    //     GUILayout.Label("ServerRPC Called Time: " + serverrpccalledtime.ToString());
    //     GUILayout.Label("ClientRPC Called Time: " + clientrpccalledtime.ToString());
    //     GUILayout.Label("Trophy Moved: " + trophyMoved.ToString());
    //     GUILayout.Label("Is Winner: " + isWinner.ToString());
    //     GUILayout.Label("Local Player Dead: " + localPlayerDead.ToString());
    // }

    private bool CheckGameOver(out Transform winner)
    {
        var alivePlayers = players.Where(x => x != null && !x.isDead).ToList();
        if (alivePlayers.Count == 1)
        {
            winner = alivePlayers[0].transform;
            return true;
        }
        winner = null;
        return false;
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }

    private void OnWatchClick()
    {
        // DOTween.Kill(Camera.main.transform);
        deathPanel.SetActive(false);
        var alivePlayers = players.Where(x => !x.isDead).ToList();
        if (alivePlayers.Count == 0) { Debug.LogWarning("when watch, No alive player"); return; }
        watchButtons.SetActive(true);
        OnNextClick();
    }

    private void OnNextClick()
    {
        var alivePlayers = players.Where(x => x != null && !x.isDead).ToList();
        if (alivePlayers.Count == 0) { Debug.LogWarning("when watch, No alive player"); return; }
        watchPlayerIndex++;
        watchPlayerIndex %= alivePlayers.Count;
        var cameraSpawn = alivePlayers[watchPlayerIndex].transform.Find("CameraSpawn");
        Camera.main.transform.SetParent(cameraSpawn, false);
        Camera.main.transform.localPosition = new(0, 0, -2);
        Camera.main.transform.localRotation = Quaternion.identity;
        watchingPlayer = alivePlayers[watchPlayerIndex];
    }

    private void OnPrevClick()
    {
        var alivePlayers = players.Where(x => x != null && !x.isDead).ToList();
        if (alivePlayers.Count == 0) { Debug.LogWarning("when watch, No alive player"); return; }
        watchPlayerIndex--;
        watchPlayerIndex %= alivePlayers.Count;
        if (watchPlayerIndex < 0) watchPlayerIndex += alivePlayers.Count;
        var cameraSpawn = alivePlayers[watchPlayerIndex].transform.Find("CameraSpawn");
        Camera.main.transform.SetParent(cameraSpawn, false);
        Camera.main.transform.localPosition = new(0, 0, -2);
        Camera.main.transform.localRotation = Quaternion.identity;
        watchingPlayer = alivePlayers[watchPlayerIndex];
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
        if (isRolling) return;
        rollSeq?.Kill();
        rollSeq = DOTween.Sequence();
        isRolling = true;
        var origPos = spellsInHandContent.transform.localPosition.y;
        rollSeq.Append(spellsInHandContent.transform.DOLocalMoveY(origPos - 150f, 0.5f));
        rollSeq.AppendCallback(RollLogic);
        rollSeq.AppendInterval(0.5f);
        rollSeq.Append(spellsInHandContent.transform.DOLocalMoveY(origPos, 0.5f));
        rollSeq.AppendCallback(() => isRolling = false);
    }

    private void RollLogic()
    {
        foreach (var preview in spellsInHandContent.GetComponentsInChildren<SpellPreview>())
            preview.Refresh();

        foreach (var spellPreview in spellsInHand)
        {
            if (spellPreview.cardStatus == CardStatus.InHand)
                spellPreview.cardStatus = CardStatus.Used;
        }
        spellsInHand = new();


        var candidates = deck.Where(x => x.cardStatus == CardStatus.Candidate).ToList();
        for (int i = 0; i < maxSpellsInHand; i++)
        {
            if (candidates.Count == 0)
            {
                // print("洗牌");
                candidates = deck.Where(x => x.cardStatus == CardStatus.Used).ToList();
                foreach (var candidate in candidates)
                    candidate.cardStatus = CardStatus.Candidate;
                candidates = deck.Where(x => x.cardStatus == CardStatus.Candidate).ToList();
                if (candidates.Count == 0) { break; }
            }
            var index = UnityEngine.Random.Range(0, candidates.Count);
            candidates[index].cardStatus = CardStatus.InHand;
            spellsInHandContent.GetChild(i).GetComponent<SpellPreview>().Init(candidates[index].spell.spellVariant);
            spellsInHand.Add(candidates[index]);
            candidates.RemoveAt(index);
        }

        foreach (var spellPreview in deck)
        {
            if (spellPreview.cardStatus == CardStatus.Candidate)
                spellPreview.SetTransparency(1f);
            else
                spellPreview.SetTransparency(0.2f);
        }
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
