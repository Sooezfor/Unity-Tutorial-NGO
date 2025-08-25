using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : NetworkBehaviour //�̷��� �ƴϰ� ���ڵ岨
{
    Vector3 moveInput;

    private void Update()
    {
        if(IsOwner) //������ ����
            transform.position += moveInput * 3f * Time.deltaTime;
    }

    private void OnMove(InputValue value)
    {
        var moveValue = value.Get<Vector2>();

        moveInput = new Vector3(moveValue.x, 0, moveValue.y);
    }
}
