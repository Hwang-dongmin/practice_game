using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 달리기 상태를 나타내는 클래스입니다.
/// 플레이어가 좌우로 이동할 때의 상태를 처리합니다.
/// </summary>
public class RunningState : IState
{
    // 상태를 제어하는 PlayerController 참조
    private PlayerController playerController;

    
    /// RunningState 생성자
    public RunningState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    

    public void OnEnter()
    {
        playerController.SetBoolAnimationTrue("isRunning");
    }
    

    public void OnUpdate()
    {
        
        CheckForIdle();
        CheckForJump();
        CheckForAttack();
        
    }

    public void OnFixedUpdate()
    {
        DoRun();
    }

    public void OnExit()
    {
        // 달리기 애니메이션 중지
        playerController.SetBoolAnimationFalse("isRunning");
    }

    private void CheckForIdle()
    {
        if (playerController.ShouldIdle())
        {
            playerController.StartIdle();
        }
    }

    private void CheckForJump()
    {
        if (playerController.ShouldJump()) 
        {
            playerController.StartJump();
        }
    }

    private void CheckForAttack()
    {
        if (playerController.ShouldAttack())
        {
            playerController.StartAttack();
        }
    }


    /// 입력 방향에 따라 캐릭터 스프라이트의 방향을 전환합니다.
    /// 사용처: OnUpdate()에서 호출
    private void ChangeSpriteDirection()
    {
        playerController.ChangeActiveSpriteDirection();
    }

    

    private void DoRun()
    {
        playerController.GiveForceToRun();
        
    }

    /// <summary>
    /// 현재 상태의 이름을 반환합니다.
    /// </summary>
    /// <returns>상태 이름</returns>
    public string StateName()
    {
        return "Running";
    }
}
