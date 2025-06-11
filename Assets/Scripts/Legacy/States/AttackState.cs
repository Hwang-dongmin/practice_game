using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AttackState : IState
{
    //For reference
    private PlayerController playerController;

    private float attackDeceleration;
    public BoxCollider2D attackPoint;
    public float attackRange;
    public List<string> attackList;
    public int attackCount;
    private bool goNextAttack;

    public AttackState(PlayerController playerController) { this.playerController = playerController; }
    public void OnEnter() 
    {
        attackPoint = playerController.attackPoint;
        AttackCollider();
        playerController.animator.SetTrigger("isAttack");
        attackDeceleration = playerController.deceleration * 5;
        attackCount = 0;
    }

    public void OnUpdate()
    {
        HandleAnimationTime();
        
    }

    private void HandleAnimationTime()
    {
        
        // 원하는 애니메이션이라면 플레이 중인지 체크
        float animTime = playerController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (playerController.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false) return;

        if (animTime == 0)
        {
            // 플레이 중이 아님
        }
        if (animTime > 0 && animTime < 0.99f)
        {
            // 애니메이션 플레이 중
            CheckNextAttack();
        }
        else if (animTime >= 0.99f)
        {
            // 애니메이션 종료
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.idleState);
        }
        
    }
 

    private void CheckNextAttack()
    {
        //다음 애니매이션 전환이 중복으로 발생하는지 확인
        if (playerController.animator.GetCurrentAnimatorStateInfo(0).IsName(attackList[attackCount]) == true)
        {
            goNextAttack = true;
        }
    }

    private void AttackCollider()
    {
        if (attackPoint.enabled == false)
        {
            attackPoint.enabled = true;
        }
        else
        {
            attackPoint.enabled = false;
        }
        
    }

    
    public void OnFixedUpdate()
    {
        AttackDeceleration();
    }
    public void AttackDeceleration()
    {
        playerController.frameVelocity.x = Mathf.MoveTowards(playerController.frameVelocity.x, 0, attackDeceleration * Time.fixedDeltaTime);

    }


    public void OnExit()
    {
        AttackCollider();

        goNextAttack = false;
    }
}
