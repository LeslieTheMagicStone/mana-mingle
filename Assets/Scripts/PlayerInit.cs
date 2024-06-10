using UnityEngine;
using Unity.Netcode;
using WarriorAnimsFREE;
using System;

public class PlayerInit : NetworkBehaviour
{
    [SerializeField] private bool offlineMode;
    public override void OnNetworkSpawn()
    {
        GameManager.instance.onStartGame.AddListener(OnStartGame);
        foreach (var variant in Enum.GetValues(typeof(PlayerVariant)))
            transform.GetChild((int)variant).gameObject.SetActive(false);

    }

    private void Start()
    {
        if (offlineMode)
        {
            Transform body = transform.GetChild(0);
            body.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
            body.transform.rotation = GameObject.FindWithTag("SpawnPoint").transform.rotation;
            body.gameObject.SetActive(true);

            Transform cameraSpawn = body.transform.Find("CameraSpawn");
            Camera.main.transform.SetParent(cameraSpawn, false);
            Camera.main.transform.localPosition = Vector3.zero;
        }
    }

    public void OnStartGame()
    {
        PlayerInfo playerInfo = GameManager.instance.playerInfos[OwnerClientId];
        Transform body = transform.GetChild((int)playerInfo.variant);
        body.gameObject.SetActive(true);
        body.transform.position = GameLogic.instance.GetSpawnPoint().position;
        body.transform.rotation = GameLogic.instance.GetSpawnPoint().rotation;
        Transform cameraSpawn = body.transform.Find("CameraSpawn");


        if (IsLocalPlayer)
        {
            Camera.main.transform.SetParent(cameraSpawn, false);
            Camera.main.transform.localPosition = Vector3.zero;
        }
    }
}
