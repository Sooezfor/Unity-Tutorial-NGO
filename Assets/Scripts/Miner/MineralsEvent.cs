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
        //gameObject.SetActive(false); 싱글 플레이일 땐 이렇게, 멀티일 때는 네트워크상에서도 없어지도록 디스폰
        GetComponent<NetworkObject>().Despawn(); //오브젝트 없애기. 디스트로이드 같은 것
    }
}
