using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class IdleState : IState
{
    // 상태를 제어하는 PlayerController 참조
    private PlayerController playerController;

    // 생성자
    public IdleState(PlayerController playerController) { this.playerController = playerController; }



    public void OnEnter()
    {
        //애니메이션 전환 불필요
        
    }

    public void OnUpdate()
    {
        //각 상태로의 전환 확인
        CheckForRun();
        CheckForJump();
        CheckForAttack();
    }



    public void OnFixedUpdate()
    {
        //마찰에의한 속력 감소
        playerController.AdjustHorizontalVelocity();
        
    }

    public void OnExit()
    {

    }

    private void CheckForRun()
    {
        if (playerController.ShouldRun())
        {
            playerController.StartRun();
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

    /// <summary>
    /// 현재 상태의 이름을 반환합니다.
    /// </summary>
    /// <returns>상태 이름</returns>
    public string StateName()
    {
        return "Idle";
    }

    

}
