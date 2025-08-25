using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] TextMeshProUGUI scoreTextUI;
    NetworkVariable<int> globalScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        globalScore.OnValueChanged += OnScoreChanged;
    }

    void OnScoreChanged(int prevValue, int newValue)
    {
        scoreTextUI.text = newValue.ToString();
    }

    public void AddScore() //글로벌 스코어를 올리는 것은 서버만 가능하도록 AddScore
    {
        if (!IsServer) //서버인 경우에만 가능하도록
            return; 

        globalScore.Value++;
    }
}
