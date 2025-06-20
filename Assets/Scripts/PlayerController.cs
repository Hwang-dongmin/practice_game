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
    public float maxSpeed;
    public float jumpPower;

    //Player Physic
    [SerializeField]
    public Vector2 frameVelocity;
    public float fallAcceleraction;
    public float maxFallSpeed;
    
    //x axis move
    public float acceleration{ get; private set; }
    public float deceleration{ get; private set; }

    //PlayerCheck
    public Vector2 inputDirection{ get; private set; }
    public bool isGround{ get; private set; }



    //CheckVariable
    public Vector2 boxSize;
    public float castDistance;



    /// <summary>
    /// 게임 오브젝트가 활성화될 때 호출됩니다.
    /// 필요한 컴포넌트들을 초기화하고 상태 머신을 생성합니다.
    /// </summary>
    private void Awake()
    {
        // 필수 컴포넌트 초기화
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 상태 머신 초기화
        stateMachine = new StateMachine(this);
    }

    /// <summary>
    /// 첫 프레임 업데이트 전에 호출됩니다.
    /// 상태 머신을 초기 상태로 설정합니다.
    /// </summary>
    private void Start()
    {
        stateMachine.StateInitialize();
    }

    /// <summary>
    /// 매 프레임 호출됩니다.
    /// 입력 감지 및 상태 머신 업데이트를 처리합니다.
    /// </summary>
    private void Update()
    {
        // 디버그용 상태 표시
        state.text = stateMachine.StateReturnCurrentState();
        
        // 입력 감지
        OnMoveInput();
        
        // 상태 머신 업데이트
        stateMachine.StateUpdate();
    }

    /// <summary>
    /// 물리 업데이트 주기마다 호출됩니다.
    /// 지면 체크, 상태 머신 물리 업데이트, 이동 적용을 처리합니다.
    /// </summary>
    private void FixedUpdate()
    {
        // 지면 체크
        IsGround();
        
        // 상태 머신 물리 업데이트
        stateMachine.StateFixedUpdate();
        
    
        // 이동 적용
        ApplyMovement();
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
            Debug.Log(rayHit.collider.name);
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

    /// <summary>
    /// 점프 입력을 감지합니다.
    /// 사용처: 점프 상태로 전환할 때 호출
    /// </summary>
    /// <returns>스페이스바가 눌렸으면 true, 아니면 false</returns>
    public bool TryJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    /// <summary>
    /// 공격 입력을 감지합니다.
    /// 사용처: 공격 상태로 전환할 때 호출
    /// </summary>
    /// <returns>Z 키가 눌렸으면 true, 아니면 false</returns>
    public bool TryAttack()
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


    public bool ShouldIdle()
    {
        if (isGround && inputDirection.x == 0)
        {
            return true;
        }
        return false;
    }

    public void StartIdle()
    {
        stateMachine.StateTransitionTo(stateMachine.idleState);
    }
    
    public bool ShouldRun()
    {
        if (isGround && inputDirection.x != 0)
        {
            return true;
        }
        return false;
    }

    public void StartRun()
    {
        stateMachine.StateTransitionTo(stateMachine.runningState);
    }

    public void SetBoolAnimationTrue(string animationName)
    {
        animator.SetBool(animationName, true);
    }

    public void SetBoolAnimationFalse(string animationName)
    {
        animator.SetBool(animationName, false);
    }

    public void GiveForceToRun()
    {
        // 입력 방향으로 힘을 가함
        rigid.AddForce(inputDirection, ForceMode2D.Impulse);

        // 최대 속도 제한
        if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
        {
            rigid.velocity = new Vector2(
                maxSpeed * Mathf.Sign(rigid.velocity.x), 
                rigid.velocity.y);
        }
    }


}
