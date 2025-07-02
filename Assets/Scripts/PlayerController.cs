using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    //Components
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public BoxCollider2D attackPoint;

    public TextMeshProUGUI state;

    public int changed=0;

    public StateMachine stateMachine;

        

    //PlayerStatus
    //Player Physic
    [SerializeField]
    private Vector2 frameVelocity;
    public float fallAcceleraction;
    public float maxFallSpeed;
    
    //x axis move
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    //y axis move
    public float jumpPower;
    public int  MaxJumpCount;
    public int jumpCount;

    //PlayerCheck
    public Vector2 inputDirection{ get; private set; }
    public bool isGround{ get; private set; }


    //GroundCheckVariable
    public Vector2 boxSize;
    public float castDistance;

    //Attack variable
    public List<string> attackList;
    public int attackCount;
    private bool isAttacking = false;

    
    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }

    public void ResetAttackCount()
    {
        attackCount = 0;
    }

    public bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }
    
 


    /// 공격 판정 콜라이더를 활성화합니다. 애니메이션 이벤트에서 호출됩니다.
    public void EnableAttackCollider()
    {
        if (attackPoint != null)
        {
            attackPoint.enabled = true;
        }
    }

    /// <summary>
    /// 공격 판정 콜라이더를 비활성화합니다. 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void DisableAttackCollider()
    {
        if (attackPoint != null)
        {
            attackPoint.enabled = false;
        }
    }


    private void Awake()
    {
        // 필수 컴포넌트 초기화
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attackPoint = GetComponent<BoxCollider2D>();

        // 상태 머신 초기화
        stateMachine = new StateMachine(this);
    }

    
    private void Start()
    {
        stateMachine.StateInitialize();
        PlayerControllerInit();
    }

    
    private void Update()
    {
        // 디버그용 상태 표시

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //state.text = stateInfo.fullPathHash.ToString();
        state.text = stateMachine.StateReturnCurrentState();
        //state.text = isAttacking.ToString();


        // 입력 감지
        OnMoveInput();
        
        // 상태 머신 업데이트
        stateMachine.StateUpdate();
    }

    
    private void FixedUpdate()
    {
        // 지면 체크
        IsGround();
        
        // 상태 머신 물리 업데이트
        stateMachine.StateFixedUpdate();
        
    
        // 이동 적용
        ApplyMovement();
    }

    private void PlayerControllerInit()
    {
        jumpCount = MaxJumpCount;
    }

    /// <summary>
    /// 플레이어의 수평 입력을 감지하여 inputDirection을 업데이트합니다.
    /// 사용처: Update()에서 매 프레임 호출됨
    /// </summary>
    private void OnMoveInput()
    {
        // 수평 방향 입력 감지 (좌: -1, 우: 1, 없음: 0)
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }

    /// <summary>
    /// 플레이어가 지면에 닿아있는지 확인합니다.
    /// 사용처: FixedUpdate()에서 호출되어 isGround 상태 업데이트, 점프 상태 전환 시 사용
    /// </summary>
    /// <returns>지면에 닿아있으면 true, 그렇지 않으면 false</returns>
    public bool IsGround()
    {
        // BoxCast를 사용하여 플레이어 아래쪽으로 캐스팅하여 지면 감지
        RaycastHit2D rayHit = Physics2D.BoxCast(
            rigid.position + Vector2.down * castDistance, 
            boxSize, 
            0f, 
            Vector2.down, 
            castDistance, 
            LayerMask.GetMask("Platform")
        );

        if (rayHit.collider != null)
        {
            //Debug.Log(rayHit.collider.name);
            isGround = true;
            return true;
        }
        else
        {
            isGround = false;
            return false;
        }
    }

    /// <summary>
    /// 유니티 에디터에서 지면 감지 영역을 시각적으로 표시합니다.
    /// 사용처: 개발 중 디버깅 용도
    /// </summary>
    private void OnDrawGizmos()
    {
        if (rigid != null)
        {
            Gizmos.DrawWireCube(rigid.position + Vector2.down * castDistance, boxSize);
        }
    }

    // 점프 입력 확인
    public bool HasJumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    // 공격 입력 확인
    public bool HasAttackInput()
    {
        return Input.GetKeyDown(KeyCode.Z);
    }

    /// <summary>
    /// 플레이어의 수평 속도를 조정합니다.
    /// 사용처: FixedUpdate()에서 호출
    /// </summary>
    public void AdjustHorizontalVelocity()
    {
        // 입력 방향에 따라 가속 또는 감속 적용
        if (inputDirection.x == 0)
        {
            // 입력이 없을 경우 서서히 정지
            frameVelocity.x = Mathf.Lerp(frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            // 입력 방향으로 가속 (부드러운 가속을 위해 Lerp 사용)
            frameVelocity.x = Mathf.Lerp(frameVelocity.x, inputDirection.x * maxSpeed, acceleration * Time.fixedDeltaTime);
        }
    }
    
    /// <summary>
    /// 입력 방향에 따라 스프라이트의 방향을 변경합니다. 직접적으로 움직임을 제어할 때 사용
    /// 사용처: 이동 시 방향 전환 시 호출
    /// </summary>
    public void ChangeActiveSpriteDirection()
    {
        if (inputDirection.x > 0)
        {
            // 오른쪽 방향 바라보기
            spriteRenderer.flipX = false;
        }
        else if (inputDirection.x < 0)
        {
            // 왼쪽 방향 바라보기
            spriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// 계산된 속도를 Rigidbody에 적용합니다.
    /// 사용처: FixedUpdate()에서 매 프레임 호출
    /// </summary>
    private void ApplyMovement()
    {
        rigid.velocity = frameVelocity;
    }


    public bool ShouldIdleStateTransition()
    {
        if (isGround && inputDirection.x == 0)
        {
            return true;
        }
        return false;
    }

    public void StartIdleStateTransition()
    {
        stateMachine.StateTransitionTo(stateMachine.idleState);
    }
    
    public bool ShouldRunStateTransition()
    {
        if (isGround && inputDirection.x != 0)
        {
            return true;
        }
        return false;
    }

    public void StartRunStateTransition()
    {
        stateMachine.StateTransitionTo(stateMachine.runState);
    }
    
    public bool ShouldAirborneStateTransition()
    {
        if (!isGround)
        {
            return true;
        }
        return false;
    }

    public void StartAirborneStateTransition()
    {
        stateMachine.StateTransitionTo(stateMachine.airborneState);
    }

    public bool ShouldAttackStateTransition()
    {
        if (HasAttackInput())
        {
            return true;
        }
        return false;
    }
    
    public void StartAttackStateTransition()
    {
        stateMachine.StateTransitionTo(stateMachine.attackState);
    }
    //공격 실행 시, 멈추기위한 용도
    public void ResetVelocity(){
        frameVelocity = Vector2.zero;
    }
    
    
    //점프 관련 함수

    //점프 수행 및 점프 횟수 차감감
    public void DoJump()
    {
        ConsumeJump();
        GiveForceToVertical(jumpPower);
    }

    //점프 가능 확인
    public bool CanJump()
    {
        if (JumpCount() > 0 && frameVelocity.y < jumpPower)
        {
            return true;
        }
        return false;
    }

    //점프 횟수 확인
    public int JumpCount()
    {
        return jumpCount;
    }

    //점프 횟수 차감
    public void ConsumeJump()
    {
        jumpCount--;
    }

    //점프 횟수 초기화
    public void ResetJumpCount()
    {
        jumpCount = MaxJumpCount;
    }

    //수직 방향 힘 적용
    public void GiveForceToVertical(float jumpForce)
    {
        frameVelocity.y = jumpForce;
    }
    

    //애니메이션 전환
    public void SetBoolAnimationTrue(string animationName)
    {
        animator.SetBool(animationName, true);
    }

    public void SetBoolAnimationFalse(string animationName)
    {
        animator.SetBool(animationName, false);
    }

    /// <summary>
    /// Rigidbody의 수직 속도를 Animator의 "VerticalSpeed" 파라미터에 전달합니다.
    /// 점프 애니메이션(상승, 정점, 하강)의 전환은 Animator Controller에서 처리합니다.
    /// </summary>
    public void UpdateJumpAnimation()
    {
        animator.SetFloat("VerticalSpeed", rigid.velocity.y);
    }

    
    //중력 처리
    public void HandleGravity()
    {
        frameVelocity.y = Mathf.MoveTowards(
            frameVelocity.y, 
            -maxFallSpeed, 
            fallAcceleraction * Time.fixedDeltaTime);
    }


}
