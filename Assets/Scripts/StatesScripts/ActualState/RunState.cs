using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 달리기 상태를 나타내는 클래스입니다.
/// 플레이어가 좌우로 이동할 때의 상태를 처리합니다.
/// </summary>
public class RunState : BaseState
{

    
    /// RunningState 생성자
    public RunState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine) { }

    

    public override void OnEnter()
    {
        playerController.SetBoolAnimationTrue("isRunning");
    }
    

    public override void OnUpdate()
    {
        //부모 클래스의 OnUpdate() 호출
        base.OnUpdate();

        // 수평 속도에 따른 Sprite 전환환
        playerController.ChangeActiveSpriteDirection();


        
        
    }

    public override void OnFixedUpdate()
    {
        DoRun();
    }

    public override void OnExit()
    {
        // 달리기 애니메이션 중지
        playerController.SetBoolAnimationFalse("isRunning");
    }

    private void DoRun()
    {
        playerController.AdjustHorizontalVelocity();
        
    }

    /// <summary>
    /// 현재 상태의 이름을 반환합니다.
    /// </summary>
    /// <returns>상태 이름</returns>
    public override string StateName()
    {
        return "Run";
    }
}
