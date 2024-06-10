using UnityEngine;
using Unity.Netcode;

public class PlayerInit : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        GameManager.instance.onStartGame.AddListener(OnStartGame);
    }

    public void OnStartGame()
    {
        PlayerInfo playerInfo = GameManager.instance.playerInfos[OwnerClientId];
        Transform body = transform.GetChild((int)playerInfo.variant);
        body.gameObject.SetActive(true);
        body.transform.position = GameLogic.instance.GetSpawnPoint().position;
        body.transform.rotation = GameLogic.instance.GetSpawnPoint().rotation;
        Transform cameraSpawn = body.transform.Find("CameraSpawn");
        Camera.main.transform.SetParent(cameraSpawn, false);
    }
}
