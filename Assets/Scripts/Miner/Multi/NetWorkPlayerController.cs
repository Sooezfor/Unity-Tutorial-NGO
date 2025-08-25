using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetWorkPlayerController : NetworkBehaviour
{
    //public enum ActionType { Idle, Move, Attack }
    //public ActionType actionType = ActionType.Idle;
    // 위에 enum 값을 아래 네트워크 변수가 대체함. Idle = 0, Move = 1, Attack =2

    NetworkVariable<int> currAnimState = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] GameObject[] animObjs;

    Rigidbody2D rb;
 
    Vector3 moveInput;

    float moveSpeed = 2f;
    float jumpPower = 7f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        rb = GetComponent<Rigidbody2D>();
        currAnimState.OnValueChanged += (prevState, newState) => UpdateAnim(newState); //prevState 는 쓰이지 않지만 형식상 필요함.

        if (!IsServer)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; //rb.isKinematic 안쓰인다고 해서 이걸로 바꿈.
        }
        if(!IsOwner)
        {
            GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();

            if(cameraFollow != null)
            {
                cameraFollow.target = transform;
            }
        }
    }

    private void Update()
    {
        if(IsOwner)
            MovementServerRpc(moveInput);
    }

    [ServerRpc]
    void MovementServerRpc(Vector2 moveDir)
    {
        if (currAnimState.Value == 2)
            return;

        if (moveDir.x == 0)
        {
            currAnimState.Value = 0;
        }
        else if (moveDir.x != 0)
        {
            rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, rb.linearVelocity.y);

            int dirX = moveDir.x < 0 ? 1 : -1;
            transform.localScale = new Vector3(dirX, 1, 1);

            currAnimState.Value = 1;           
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (IsOwner)
            JumpServerRpc();
    }

    [ServerRpc]
    void AttackServerRpc()
    {
        StartCoroutine(AttackRoutine());
    }

    [ServerRpc]
    void JumpServerRpc()
    {
        rb.AddForceY(jumpPower, ForceMode2D.Impulse);

    }
    void OnAttack()
    {
        if (IsOwner)
        {
            if (currAnimState.Value != 2)
                AttackServerRpc();
        }
    }

    IEnumerator AttackRoutine()
    {
        //공격
        currAnimState.Value = 2;      
        yield return new WaitForSeconds(1f);
        //아이들              
        currAnimState.Value = 0;

    }

    //[ServerRpc]
    //void UpdateStateServerRpc(int newState)
    //{
    //    currAnimState.Value = newState;
    //}

    void UpdateAnim(int newState)
    {
        for (int i = 0; i < animObjs.Length; i++)
            animObjs[i].SetActive(i == newState);
    }
}
