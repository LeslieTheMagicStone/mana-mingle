using UnityEngine;
using Unity.Netcode;
using WarriorAnimsFREE;
using System;

public class PlayerInit : NetworkBehaviour
{
    [SerializeField] private bool offlineMode;
    readonly Vector3 farAway = new(1000, 0, 0);
    private bool hasGameStart = false;
    public override void OnNetworkSpawn()
    {
        GameManager.instance.onStartGame.AddListener(OnStartGame);

        foreach (var variant in Enum.GetValues(typeof(PlayerVariant)))
        {
            Transform tr = transform.GetChild((int)variant);
            // tr.transform.position = farAway;
            // Remove control from non-owners
            if (!IsLocalPlayer)
            {
                Destroy(tr.GetComponent<WarriorInputController>());
                Destroy(tr.GetComponent<WarriorMovementController>());
                Destroy(tr.GetComponent<WarriorController>());
                Destroy(tr.GetComponent<SuperCharacterController>());
                Destroy(tr.GetComponent<AnimatorParentMove>());
                Destroy(tr.GetComponent<WarriorTiming>());
                Destroy(tr.GetComponent<Backpack>());
                Destroy(tr.GetComponent<PickerLogic>());
            }
            else
            {
                tr.GetComponent<WarriorInputController>().enabled = false;
                tr.GetComponent<WarriorMovementController>().enabled = false;
                tr.GetComponent<WarriorController>().enabled = false;
                tr.GetComponent<SuperCharacterController>().enabled = false;
                tr.GetComponent<AnimatorParentMove>().enabled = false;
                tr.GetComponent<WarriorTiming>().enabled = false;
                tr.GetComponent<PickerLogic>().enabled = false;
            }
        }
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
        hasGameStart = true;
        PlayerInfo playerInfo = GameManager.instance.playerInfos[OwnerClientId];

        // Not selected variant, then set active false.
        foreach (PlayerVariant variant in Enum.GetValues(typeof(PlayerVariant)))
        {
            Transform tr = transform.GetChild((int)variant);
            tr.gameObject.SetActive(variant == playerInfo.variant);
        }

        // Set position and rotation and camera to the selected body.
        Transform body = transform.GetChild((int)playerInfo.variant);
        Transform spawnPoint = GameLogic.instance.GetSpawnPoint((int)NetworkManager.LocalClientId);
        body.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        GameLogic.instance.players.Add(body.GetComponent<Damageable>());

        if (IsLocalPlayer)
        {
            Transform cameraSpawn = body.transform.Find("CameraSpawn");
            body.GetComponent<WarriorInputController>().enabled = true;
            body.GetComponent<WarriorMovementController>().enabled = true;
            body.GetComponent<WarriorController>().enabled = true;
            body.GetComponent<SuperCharacterController>().enabled = true;
            body.GetComponent<AnimatorParentMove>().enabled = true;
            body.GetComponent<WarriorTiming>().enabled = true;
            body.GetComponent<PickerLogic>().enabled = true;
            Camera.main.transform.SetParent(cameraSpawn, false);
            Camera.main.transform.localPosition = Vector3.zero;

            var mapCam = GameObject.Find("MapCamera");
            mapCam.transform.SetParent(body);
            Vector3 xzZero = new(0, mapCam.transform.localPosition.y, 0);
            mapCam.transform.localPosition = xzZero;
            var mapPoint = body.Find("MapPoint");
            mapPoint.gameObject.SetActive(true);
        }
    }
}
