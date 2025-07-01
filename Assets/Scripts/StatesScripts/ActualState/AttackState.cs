using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// 플레이어의 공격 상태를 나타내는 클래스입니다.
/// 플레이어가 공격할 때의 상태를 처리합니다.
/// </summary>
public class AttackState : BaseState
{
    

    public AttackState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine) { }
    
    
    public override void OnEnter() 
    {
        //공격중 다른 State로의 전환을 막기위한 isAttacking 변수 설정
        playerController.SetIsAttacking(true);
        playerController.animator.SetTrigger("isAttack");
        Debug.Log("AttackState Enter");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //
    }

 


    public override void OnFixedUpdate()
    {
    }
    
    public override void OnExit()
    {
        Debug.Log("AttackState Exit");
    }

    public override string StateName()
    {
        return "Attack";
    }
}
