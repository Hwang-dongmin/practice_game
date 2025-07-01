using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 상태 머신 클래스로, 플레이어의 상태를 관리합니다.
/// 상태 패턴을 사용하여 각 상태 간의 전환을 처리합니다.
/// </summary>
public class StateMachine
{
    /// <summary>현재 상태</summary>
    public IState currentState { get; private set; }

    /// <summary>상태 머신을 소유한 플레이어 컨트롤러</summary>
    public PlayerController playerController;

    // 상태 인스턴스들
    public IdleState idleState;
    public RunState runState;
    public AirborneState airborneState;
    public AttackState attackState;

    /// <summary>
    /// 상태 머신을 초기화하고 모든 상태를 생성합니다.
    /// </summary>
    /// <param name="_playerController">상태 머신을 소유할 플레이어 컨트롤러</param>
    public StateMachine(PlayerController _playerController)
    {
        this.playerController = _playerController;
        this.runState = new RunState(_playerController, this);
        this.idleState = new IdleState(_playerController, this);
        this.airborneState = new AirborneState(_playerController, this);
        this.attackState = new AttackState(_playerController, this);
    }

    /// <summary>
    /// 상태 머신을 초기 상태로 초기화합니다.
    /// 사용처: 게임 시작 시 PlayerController의 Start()에서 호출
    /// </summary>
    public void StateInitialize()
    {
        currentState = idleState;
        currentState.OnEnter();
    }

    public string StateReturnCurrentState()
    {
        return currentState.StateName();
    }

    /// <summary>
    /// 현재 상태에서 다른 상태로 전환합니다.
    /// 사용처: 각 상태 클래스에서 상태 전환이 필요할 때 호출
    /// </summary>
    /// <param name="nextState">전환할 다음 상태</param>
    public void StateTransitionTo(IState nextState)
    {
        currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();

        // 상태 변경 디버깅을 위한 카운터 증가
        playerController.changed += 1;
    }

    /// <summary>
    /// 현재 상태의 업데이트를 호출합니다.
    /// 사용처: PlayerController의 Update()에서 매 프레임 호출
    /// </summary>
    public void StateUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    /// <summary>
    /// 현재 상태의 물리 업데이트를 호출합니다.
    /// 사용처: PlayerController의 FixedUpdate()에서 고정 프레임마다 호출
    /// </summary>
    public void StateFixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }

}
