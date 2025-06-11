using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine
{
    public IState currentState { get; private set; }

    public PlayerController playerController;

    public IdleState idleState;
    public RunningState runningState;
    public JumpState jumpState;
    public AttackState attackState;



    public StateMachine(PlayerController _playerController)
    {
        this.playerController = _playerController;
        this.runningState = new RunningState(_playerController);
        this.idleState = new IdleState(_playerController);
        this.jumpState = new JumpState(_playerController);
        this.attackState = new AttackState(_playerController);
        
    }

    public void StateInitialize()
    {
        currentState= idleState;
        currentState.OnEnter();
    }

    public void StateTransitionTo(IState nextState)
    {
        currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();

        //For Checking if the state changed
        playerController.changed += 1;
    }

    public void StateUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
        
    }

    public void StateFixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }

}
