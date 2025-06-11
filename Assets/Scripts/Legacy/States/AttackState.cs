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
        
        // ���ϴ� �ִϸ��̼��̶�� �÷��� ������ üũ
        float animTime = playerController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (playerController.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false) return;

        if (animTime == 0)
        {
            // �÷��� ���� �ƴ�
        }
        if (animTime > 0 && animTime < 0.99f)
        {
            // �ִϸ��̼� �÷��� ��
            CheckNextAttack();
        }
        else if (animTime >= 0.99f)
        {
            // �ִϸ��̼� ����
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.idleState);
        }
        
    }
 

    private void CheckNextAttack()
    {
        //���� �ִϸ��̼� ��ȯ�� �ߺ����� �߻��ϴ��� Ȯ��
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
