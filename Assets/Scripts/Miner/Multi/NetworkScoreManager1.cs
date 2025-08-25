using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkScoreManager1 : NetworkBehaviour
{
    public static NetworkScoreManager1 Instance;

    [SerializeField] TextMeshProUGUI scoreUi;

    NetworkVariable<int> score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        score.OnValueChanged += OnScoreChanged;

        scoreUi.text = score.Value.ToString();

    }

    void OnScoreChanged(int prevValue, int newValue)
    {
        scoreUi.text = newValue.ToString();
    }

    public void AddScore()
    {
        if (!IsServer)
            return;

        score.Value++;
    }
}
