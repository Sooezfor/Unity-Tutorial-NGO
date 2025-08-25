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

    public void AddScore() //�۷ι� ���ھ �ø��� ���� ������ �����ϵ��� AddScore
    {
        if (!IsServer) //������ ��쿡�� �����ϵ���
            return; 

        globalScore.Value++;
    }
}
