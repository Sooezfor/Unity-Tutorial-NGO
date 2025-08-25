using StarterAssets;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAmatureMover : NetworkBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] StarterAssetsInputs starterAsset;
    [SerializeField] ThirdPersonController controller;
    [SerializeField] Transform playerRoot;

    [SerializeField] GameObject bombPrefab;

    private void Awake()
    {
        cc.enabled = false;
        playerInput.enabled = false;
        starterAsset.enabled = false;
        controller.enabled = false;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsOwner)
        {
            cc.enabled = true;
            playerInput.enabled = true;
            starterAsset.enabled = true;
            controller.enabled = true;

            var cinemachine = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineCamera>();
            cinemachine.Target.TrackingTarget = playerRoot;
        }
    }
    private void Update()
    {
        if (!IsOwner)
            return;

        if(Input.GetKeyDown(KeyCode.Return)) //리턴은 엔터키
        {
             AddScoreServerRpc();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            ThrowBombServerRpc();

        }
    }

    [ServerRpc] //서버에서 관리하기 때문에 서버 전용으로 만들어줌
    void AddScoreServerRpc()
    {          
        ScoreManager.Instance.AddScore();
    }

    [ServerRpc]
    void ThrowBombServerRpc()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}
