using Unity.Netcode;
using UnityEngine;

public class MineralsEvent : NetworkBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Player") && IsOwner)
        {      
            AddScoreServerRpc();
        }
    }

    [ServerRpc]
    void AddScoreServerRpc()
    {
        NetworkScoreManager1.Instance.AddScore();
        //gameObject.SetActive(false); �̱� �÷����� �� �̷���, ��Ƽ�� ���� ��Ʈ��ũ�󿡼��� ���������� ����
        GetComponent<NetworkObject>().Despawn(); //������Ʈ ���ֱ�. ��Ʈ���̵� ���� ��
    }
}
