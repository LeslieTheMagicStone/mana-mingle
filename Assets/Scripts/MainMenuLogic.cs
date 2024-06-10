using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
    [SerializeField] private Button create;
    [SerializeField] private Button join;
    [SerializeField] private Button quit;
    [SerializeField] private TMP_InputField ip;

    private void Awake()
    {
        create.onClick.AddListener(OnCreateClick);
        join.onClick.AddListener(OnJoinClick);
        quit.onClick.AddListener(OnQuitClick);
    }

    private void SetTransformData()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
        transport.SetConnectionData(ip.text, 7777);
    }

    private void OnJoinClick()
    {
        SetTransformData();
        NetworkManager.Singleton.StartClient();
    }

    private void OnCreateClick()
    {
        SetTransformData();
        NetworkManager.Singleton.StartHost();
        SceneController.instance.LoadScene("Lobby");
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }
}
