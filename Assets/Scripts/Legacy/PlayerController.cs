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
    public Vector2 frameVelocity;
    public float fallAcceleraction;
    public float maxFallSpeed;
    
    //x axis move
    public float acceleration;
    public float deceleration;

    //PlayerCheck
    public Vector2 inputDirection;
    public bool isGround;


    //CheckVariable
    public Vector2 boxSize;
    public float castDistance;

    private void OnMoveInput()
    {
        //Check the input direction
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }


    

    public bool IsGround()
    {


        //RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, castDistance, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHit = Physics2D.BoxCast(rigid.position + Vector2.down*castDistance, boxSize, 0f, Vector2.down,castDistance, LayerMask.GetMask("Platform"));

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(rigid.position + Vector2.down * castDistance,boxSize);
    }

    public bool TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #region Common

    //this function is from IdleState. The reason why it came here is to reuse function in jumpstate
    public void ForceToRunning()
    {
        //Running logic
        //Add Force to Player Using the inputDirection of PlayerController
        

        //Restrict the Speed using maxSpeed

        if (inputDirection.x == 0)
        {
            frameVelocity.x = Mathf.Lerp(frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            
        }
        else
        {
            //frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, inputDirection.x *maxSpeed, acceleration * Time.fixedDeltaTime);
            // 현재 속도가 amxSpeed보다 높을 경우 속도가 오히려 낮아질 수 있음. max(a,b) 사용
            frameVelocity.x = Mathf.Lerp(frameVelocity.x, inputDirection.x * maxSpeed, acceleration * Time.fixedDeltaTime);
        }

        /*
        if (Mathf.Abs(frameVelocity.x) < maxSpeed)
        {
            rigid.AddForce(inputDirection, ForceMode2D.Impulse);
        }
        if (Mathf.Abs(frameVelocity.x) >= maxSpeed)
        {
            frameVelocity.x = maxSpeed * Mathf.Sign(frameVelocity.x);
        }
        */

    
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

    

    #endregion



    private void ApplyMovement()
    {
        rigid.velocity = frameVelocity;
    }


    private void Awake()
    {
        // 각 컴포넌트 찾기
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator= GetComponent<Animator>();

        stateMachine = new StateMachine(this);
    }

    private void Start()
    {
        stateMachine.StateInitialize();
    }

    private void Update()
    {

        state.text = changed.ToString();
        OnMoveInput();
        stateMachine.StateUpdate();
    }

    private void FixedUpdate()
    {
        //isGround -> CollisionCheck: need to change function someday
        IsGround();
        stateMachine.StateFixedUpdate();
        ApplyMovement();
        
    }



}
