using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private Button create;
    [SerializeField] private Button join;
    [SerializeField] private Button quit;
    [SerializeField] private TMP_InputField ip;
    [SerializeField] private Button changeLanguageButton;

    private UnityTransport transport;

    private async void Awake()
    {
        create.onClick.AddListener(OnCreateClick);
        join.onClick.AddListener(OnJoinClick);
        quit.onClick.AddListener(OnQuitClick);
        changeLanguageButton.onClick.AddListener(() => LocalizationManager.Instance.NextLanguage());

        transport = FindObjectOfType<UnityTransport>();

        buttons.SetActive(false);

        await Authenticate();

        buttons.SetActive(true);
    }

    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async void OnJoinClick()
    {
        buttons.SetActive(false);

        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(ip.text);

        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }

    private async void OnCreateClick()
    {
        buttons.SetActive(false);

        Allocation a = await RelayService.Instance.CreateAllocationAsync(5);
        GameManager.instance.joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();

        SceneController.instance.LoadScene(Scenes.Lobby);
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }
}
