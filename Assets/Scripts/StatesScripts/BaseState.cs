using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
    protected PlayerController playerController;
    protected StateMachine stateMachine;

    public BaseState(PlayerController playerController, StateMachine stateMachine)
    {
        this.playerController = playerController;
        this.stateMachine = stateMachine;
    }
    
    public virtual void OnEnter() { }
    public virtual void OnUpdate()
    {
        //각 상태 전환 체크
        CheckForIdleStateTransition();
        CheckForRunStateTransition();
        CheckForAirbornStateTransition();
        CheckForAttackStateTransition();
        //점프 체크
        CheckForJump();
    }
    public virtual void OnFixedUpdate() { }
    public virtual void OnExit() { }
    public virtual string StateName() { return "BaseState"; }


    //State Transition Check 

    private bool IsCurrentState(IState state)
    {
        return stateMachine.currentState == state;
    }

    private void CheckForIdleStateTransition()
    {
        if (!playerController.IsAttacking() && playerController.ShouldIdleStateTransition() && !IsCurrentState(stateMachine.idleState))
        {
            playerController.StartIdleStateTransition();
        }
    }
    
    private void CheckForRunStateTransition()
    {
        if (!playerController.IsAttacking() && playerController.ShouldRunStateTransition() && !IsCurrentState(stateMachine.runState))
        {
            playerController.StartRunStateTransition();
        }
    }
    private void CheckForJump() 
    {
        if (!playerController.IsAttacking() && playerController.HasJumpInput()&& playerController.CanJump())
        {
            playerController.DoJump();
        }
    }
    private void CheckForAirbornStateTransition()
    {
        if (!playerController.IsAttacking() && playerController.ShouldAirborneStateTransition() && !IsCurrentState(stateMachine.airborneState))
        {
            playerController.StartAirborneStateTransition();
        }
    }
    private void CheckForAttackStateTransition()
    {
        if (!playerController.IsAttacking() && playerController.ShouldAttackStateTransition() && !IsCurrentState(stateMachine.attackState))
        {
            playerController.StartAttackStateTransition();
        }
    }
}
