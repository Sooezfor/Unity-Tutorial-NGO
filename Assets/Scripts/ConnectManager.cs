using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : MonoBehaviour
{
    [SerializeField] Button serverButton;
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;

    private void Awake()
    {
        serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }
}
