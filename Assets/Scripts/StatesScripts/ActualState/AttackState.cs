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
    private bool _bufferedAttack; // 다음 공격 입력을 기억하는 변수

    public AttackState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine) { }
    
    public override void OnEnter()
    {
        //로직 실행 전 변수 초기화
        //생각: base.onUpdate를 실행하지 않는 시점에서 IsAttacking의 존재 의미가 있나?
        playerController.SetIsAttacking(true);
        _bufferedAttack = false; 

        // 공격 실행 시, 멈추기위한 용도
        playerController.ResetVelocity();
        
        // 이전 공격이 마지막 연속 공격일 경우 다시 첫 공격으로 전환을 위한 리셋
        if (playerController.attackCount >= playerController.attackList.Count)
        {
            playerController.ResetAttackCount();
        }

        // 현재 콤보에 맞는 애니메이션 재생
        playerController.animator.Play(playerController.attackList[playerController.attackCount]);
    }

    public override void OnUpdate()
    {
        // 공격 애니메이션 중에 다음 공격 입력을 받으면 버퍼링
        if (playerController.HasAttackInput())
        {
            _bufferedAttack = true;
        }

        // 현재 재생 중인 애니메이션이 끝났는지 확인
        string currentAnimation = playerController.attackList[playerController.attackCount];
        if (playerController.IsAnimationFinished(currentAnimation))
        {
            // 다음 콤보 공격이 가능하고, 입력이 버퍼링 되었다면
            if (_bufferedAttack && playerController.attackCount + 1 < playerController.attackList.Count)
            {
                playerController.attackCount++;
                stateMachine.StateTransitionTo(stateMachine.attackState); // 다음 공격을 위해 AttackState로 재진입
            }
            else
            {
                // 입력이 없었으면 attackCount를 0으로 초기화하고 Idle 상태로 전환
                playerController.ResetAttackCount();
                playerController.animator.Play("Idle");
                stateMachine.StateTransitionTo(stateMachine.idleState);
            }
        }
    }

    public override void OnFixedUpdate()
    {
        // 공격 중에는 움직임을 멈추거나, 특정 움직임을 제어할 수 있습니다.
        // 예: playerController.StopMovement();
    }

    public override void OnExit()
    {
        //로직 실행 후 변수 초기화
        playerController.SetIsAttacking(false);
    }

    public override string StateName()
    {
        return "Attack";
    }
}
