using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class IdleState : BaseState
{

    // 생성자
    public IdleState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine) { }



    public override void OnEnter()
    {
        //애니메이션 전환 불필요
        Debug.Log("IdleState Enter");
        
    }

    public override void OnUpdate()
    {
        //부모 클래스의 OnUpdate() 호출
        base.OnUpdate();
    }



    public override void OnFixedUpdate()
    {
        //마찰에의한 속력 감소
        playerController.AdjustHorizontalVelocity();
        
    }

    public override void OnExit()
    {

    }


    /// <summary>
    /// 현재 상태의 이름을 반환합니다.
    /// </summary>
    /// <returns>상태 이름</returns>
    public override string StateName()
    {
        return "Idle";
    }

    

}
