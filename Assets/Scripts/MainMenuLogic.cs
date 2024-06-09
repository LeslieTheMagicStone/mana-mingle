using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
    [SerializeField] private TMP_InputField ip;

    private void SetTransformData()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
        transport.SetConnectionData(ip.text, 7777);
    }

    public void OnJoinClick()
    {
        SetTransformData();
        NetworkManager.Singleton.StartClient();
    }

    public void OnCreateClick()
    {
        SetTransformData();
        NetworkManager.Singleton.StartHost();

        GameManager.instance.LoadScene("Lobby");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
