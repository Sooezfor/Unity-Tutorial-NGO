using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject[] animObjs;

    bool isAttack = false;
    Vector3 moveInput;
    Rigidbody2D rb;
    float moveSpeed = 2f;
    float jumpPower = 7f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (isAttack)
            return;

        if(moveInput.x == 0)
        {
            SetAnimObject(0);
        }
        else if(moveInput.x != 0)
        {
            int dirX = moveInput.x < 0 ? 1 : -1;
            transform.localScale = new Vector3(dirX, 1, 1);
            SetAnimObject(1);
           
            transform.position += moveInput * moveSpeed * Time.deltaTime;
        }
           
    }

    void OnMove(InputValue value)
    {
        var moveValue = value.Get<Vector2>();

        moveInput = new Vector3(moveValue.x, transform.position.y, 0);
    }

    void OnJump()
    {
        rb.AddForceY(jumpPower, ForceMode2D.Impulse);
    }

    void OnAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttack = true;
        SetAnimObject(2); //공격

        yield return new WaitForSeconds(1f);

        SetAnimObject(0); //아이들로 돌아오기 
        isAttack = false;
    }

    void SetAnimObject(int index)
    {
        for(int i = 0; i< animObjs.Length; i++)
        {
            animObjs[i].SetActive(false);
        }
        animObjs[index].SetActive(true);
    }
}
