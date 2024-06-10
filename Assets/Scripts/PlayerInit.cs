using UnityEngine;
using Unity.Netcode;

public class PlayerInit : NetworkBehaviour
{
    [SerializeField] private bool offlineMode;
    public override void OnNetworkSpawn()
    {
        GameManager.instance.onStartGame.AddListener(OnStartGame);
    }

    private void Start()
    {
        if (offlineMode)
        {
            Transform body = transform.GetChild(0);
            body.gameObject.SetActive(true);
            body.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
            body.transform.rotation = GameObject.FindWithTag("SpawnPoint").transform.rotation;
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
            Camera.main.transform.SetParent(cameraSpawn, false);
    }
}
