using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어의 점프 상태를 나타내는 클래스입니다.
/// 플레이어가 점프하거나 공중에 있을 때의 상태를 처리합니다.
/// </summary>
public class JumpState : IState
{
    // 상태를 제어하는 PlayerController 참조
    private PlayerController playerController;

    // ForceToRun() 재사용을 위한 상태 참조
    private IState runningState;



    /// <summary>
    /// JumpState 생성자
    /// </summary>
    /// <param name="playerController">상태를 제어할 PlayerController</param>
    public JumpState(PlayerController playerController) {
        this.playerController = playerController;  
    }



    /// <summary>
    /// 상태가 시작될 때 호출됩니다.
    /// 사용처: 상태 머신에서 상태 전환 시 호출
    /// </summary>
    public void OnEnter()
    {
        if (CheckPlayerJump())
        {
            JumpFromGround();
        }

        playerController.animator.SetBool("isJumping", true);
    }


    /// <summary>
    /// 플레이어가 점프를 시작했는지 확인합니다.
    /// 사용처: OnEnter()에서 호출
    /// </summary>
    /// <returns>지면에서 점프를 시작했으면 true, 그렇지 않으면 false</returns>
    private bool CheckPlayerJump()
    {
        if (playerController.isGround == true)
        {
            // 지면에서 점프 시작
            //playerController.isGround = false;
            return true;
        }
        else
        {
            // 그냥 낙하 중
            return false;
        }
    }
    /// <summary>
    /// 지면에서 점프를 수행합니다.
    /// 사용처: CheckPlayerJump()에서 호출
    /// </summary>
    private void JumpFromGround()
    {
        playerController.frameVelocity.y = playerController.jumpPower;
    }
    /// <summary>
    /// 매 프레임 호출됩니다.
    /// 사용처: 상태별 업데이트 로직 처리
    /// </summary>
    public void OnUpdate()
    {
        // y축 속도에 따라 점프 애니메이션 조정
        ReviseJumpAnimation();
        
        // 착지 여부 확인 후 Idle 상태로 전환
        TransToIdle();

        // 스프라이트 방향 전환
        playerController.ChangeSpriteDirection();
    }

    /// <summary>
    /// 점프 애니메이션을 조정합니다.
    /// 사용처: OnUpdate()에서 호출
    /// TODO: 구현 필요
    /// </summary>
    private void ReviseJumpAnimation()
    {
        // 구현 필요
    }

    /// <summary>
    /// Idle 상태로 전환 여부를 확인하고 전환합니다.
    /// 사용처: OnUpdate()에서 호출
    /// </summary>
    private void TransToIdle()
    {
        if (playerController.isGround && playerController.rigid.velocity.y <= 0f)
        {
            playerController.animator.SetBool("isJumping", false);
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.idleState);
        }
    }

    /// <summary>
    /// 물리 업데이트 주기마다 호출됩니다.
    /// 사용처: 상태 머신의 StateFixedUpdate()에서 호출
    /// </summary>
    public void OnFixedUpdate()
    {
        // 달리기 힘 적용
        playerController.GiveForceToRun();
        // 중력 처리
        HandleGravity();
    }

    /// <summary>
    /// 중력에 의한 낙하를 처리합니다.
    /// 사용처: OnFixedUpdate()에서 호출
    /// </summary>
    private void HandleGravity()
    {
        // 현재 y 속도를 최대 낙하 속도까지 가속
        playerController.frameVelocity.y = Mathf.MoveTowards(
            playerController.frameVelocity.y, 
            -playerController.maxFallSpeed, 
            playerController.fallAcceleraction * Time.fixedDeltaTime);
    }


    public void OnExit()
    {
        // 상태 종료 시 필요한 정리 작업
    }

    /// <summary>
    /// 현재 상태의 이름을 반환합니다.
    /// </summary>
    /// <returns>상태 이름</returns>
    public string StateName()
    {
        return "Jump";
    }
}
