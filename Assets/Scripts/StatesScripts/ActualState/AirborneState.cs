using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AirborneState : BaseState
{

    // ForceToRun() 재사용을 위한 상태 참조
    private IState runningState;


    public AirborneState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine) { }


    public override void OnEnter()
    {
        Debug.Log("AirborneState Enter");
        playerController.SetBoolAnimationTrue("isJumping");
    }

    public override void OnUpdate()
    {
        //부모 클래스의 OnUpdate() 호출
        base.OnUpdate();    

        // 수직 속도에 따라 점프 애니메이션을 업데이트합니다.
        playerController.UpdateJumpAnimation();
         // 스프라이트 방향 전환
        playerController.ChangeActiveSpriteDirection();
    }

    public override void OnFixedUpdate()
    {
        // 달리기 힘 적용
        playerController.AdjustHorizontalVelocity();
        // 중력 처리
        playerController.HandleGravity();

        

    }

    public override void OnExit()
    {
        Debug.Log("AirborneState Exit");
        playerController.ResetJumpCount();
        playerController.SetBoolAnimationFalse("isJumping");
    }

    public override string StateName()
    {
        return "Airborne";
    }

}
    

    


    
