using Unity.Netcode;
using UnityEngine;

public class NetworkBomb : NetworkBehaviour
{
    float timer = 0f;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        base.OnNetworkSpawn();        
    }

    private void Update()
    {
        transform.Translate(Vector3.up * 10f * Time.deltaTime);
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            timer = 0f;
            ActiveBombServerRpc();
        }
    }

    [ServerRpc]
    void ActiveBombServerRpc()
    {
        GetComponent<NetworkObject>().Despawn(); //일반 디스트로이가 아니라 디스폰 해야함. 동기화 하기 위해
        Debug.Log("터졌습니다.");       
    }
}
