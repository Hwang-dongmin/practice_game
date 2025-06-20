using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{

    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public AttackManager AtkManager;

    // 체크 변수
    private bool isGrounded = false;
    private bool isAttacking = false;
    private bool isDefending = false;


    // 이동 변수
    private Vector2 inputDirection;
    public float moveSpeed = 5f;
    public float maxSpeed = 10f; 
    //x axis move
    public float acceleration;
    public float deceleration;

    public float jumpPower;


    public int health = 100;

    // 물리 변수
    public Vector2 frameVelocity;
    public float fallAcceleraction;
    public float maxFallSpeed;

    //RayCast 2D 변수
    public Vector2 boxSize;
    public float castDistance;



    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        AtkManager = new AttackManager(this);
    }

    void Update()
    {

        //Input
        InputDirection();

        //Check
        CheckGrounded();

        //Sprite
        ChangeSpriteDirection();
        UpdateAnimation();

        Move();
        Jump();
        Attack();
        JumpAttack();
        Defend();

        HandleGravity();
        ApplyMovement();
    }
    private void InputDirection()
    {
        //Check the input direction
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }


    void Move()
    {
        if (!isAttacking && !isDefending)
        {
            if (inputDirection.x == 0)
            {
                animator.SetBool("isRunning", false);

                frameVelocity.x = Mathf.Lerp(frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);

            }
            else
            {
                animator.SetBool("isRunning", true);

                //frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, inputDirection.x *maxSpeed, acceleration * Time.fixedDeltaTime);
                // 현재 속도가 amxSpeed보다 높을 경우 속도가 오히려 낮아질 수 있음. max(a,b) 사용
                frameVelocity.x = Mathf.Lerp(frameVelocity.x, inputDirection.x * maxSpeed, acceleration * Time.fixedDeltaTime);
            }
        }   
    }

    public void ChangeSpriteDirection()
    {
        if (inputDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void UpdateAnimation()
    {
        // 애니메이션 상태를 업데이트
        animator.SetBool("isGrounded", isGrounded);
    }


    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking && !isDefending)
        {
            frameVelocity.y = jumpPower;
        }
    }


    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking && !isDefending && isGrounded)
        {
            isAttacking = true;
            // 공격 애니메이션 시작
            animator.SetTrigger("isAttack"); // 공격 애니메이션 트리거
            Invoke("EndAttack", 0.5f); // 예: 0.5초 후에 공격 종료
        }
    }

    void JumpAttack()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking && !isDefending && !isGrounded)
        {
            isAttacking = true;
            // 점프 공격 애니메이션 시작
            Invoke("EndAttack", 0.5f); // 예: 0.5초 후에 공격 종료
        }
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    void Defend()
    {
        if (Input.GetButtonDown("Fire2") && !isAttacking)
        {
            isDefending = true;
            // 방어 애니메이션 시작
            Invoke("EndDefend", 1f); // 예: 1초 후에 방어 종료
        }
    }

    void EndDefend()
    {
        isDefending = false;
    }

    

    public void TakeDamage(int damage)
    {
        if (!isDefending)
        {
            health -= damage; // 데미지 적용
            Debug.Log("피격! 남은 생명: " + health);

            // 피격 애니메이션 시작 (예: Animator에 Trigger 설정)
            // animator.SetTrigger("Hit");

            if (health <= 0)
            {
                Die(); // 생명이 0 이하일 경우 사망 처리
            }
        }
        else
        {
            Debug.Log("방어 성공! 데미지 감소");
        }
    }
    void Die()
    {
        Debug.Log("플레이어 사망");
        // 사망 애니메이션 시작 및 게임 오버 처리
        // 예: animator.SetTrigger("Die");
        // 게임 종료 로직 추가
    }



    #region CHECK METHOD
    public bool CheckGrounded()
    {

        //RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, castDistance, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHit = Physics2D.BoxCast(rigid.position + Vector2.down * castDistance, boxSize, 0f, Vector2.down, castDistance, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null)
        {
            Debug.Log(rayHit.collider.name);
            isGrounded = true;
            return true;
        }
        else
        {
            isGrounded = false;
            return false;
        }
    }
    
    private void OnDrawGizmos()
    {
        //BoxCast 범위 확인
        Gizmos.DrawWireCube(rigid.position + Vector2.down * castDistance, boxSize);
    }
    
    #endregion

    private void HandleGravity()
    {
        //if JumpingState
        //change velocity.y from 'current' to max in a speed of fallAcceleration
        frameVelocity.y = Mathf.MoveTowards(frameVelocity.y, -maxFallSpeed, fallAcceleraction * Time.fixedDeltaTime);

    }
    private void ApplyMovement()
    {
        rigid.velocity = frameVelocity;
    }
}
