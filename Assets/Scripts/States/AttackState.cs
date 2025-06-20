using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// 플레이어의 공격 상태를 나타내는 클래스입니다.
/// 플레이어가 공격할 때의 상태를 처리합니다.
/// </summary>
public class AttackState : IState
{
    // 상태를 제어하는 PlayerController 참조
    private PlayerController playerController;

    // 공격 시 감속도
    private float attackDeceleration;
    
    // 공격 범위를 나타내는 콜라이더
    public BoxCollider2D attackPoint;
    
    // 공격 범위
    public float attackRange;
    
    // 공격 애니메이션 리스트
    public List<string> attackList;
    
    // 현재 공격 카운트
    public int attackCount;
    
    // 다음 공격으로 전환 가능 여부
    private bool goNextAttack;

    /// <summary> 
    /// AttackState 생성자
    /// </summary>
    /// <param name="playerController">상태를 제어할 PlayerController</param>
    public AttackState(PlayerController playerController) { this.playerController = playerController; }
    /// <summary>
    /// 상태가 시작될 때 호출됩니다.
    /// 사용처: 상태 머신에서 상태 전환 시 호출
    /// </summary>
    public void OnEnter() 
    {
        attackPoint = playerController.attackPoint;
        AttackCollider();
        playerController.animator.SetTrigger("isAttack");
        attackDeceleration = playerController.deceleration * 5;
        attackCount = 0;
    }

    /// <summary>
    /// 매 프레임 호출됩니다.
    /// 사용처: 상태별 업데이트 로직 처리
    /// </summary>
    public void OnUpdate()
    {
        HandleAnimationTime();
    }

    /// <summary>
    /// 애니메이션 재생 시간을 처리합니다.
    /// 사용처: OnUpdate()에서 호출
    /// </summary>
    private void HandleAnimationTime()
    {
        float animTime = playerController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        // Idle 상태가 아닌 경우 리턴
        if (playerController.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false) return;

        if (animTime == 0)
        {
            // 애니메이션 시작 시 처리 (필요 시 구현)
        }
        
        // 애니메이션 재생 중
        if (animTime > 0 && animTime < 0.99f)
        {
            // 다음 공격 입력 확인
            CheckNextAttack();
        }
        // 애니메이션 종료 시
        else if (animTime >= 0.99f)
        {
            // Idle 상태로 전환
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.idleState);
        }
    }
 

    /// <summary>
    /// 다음 공격으로 전환 가능한지 확인합니다.
    /// 사용처: HandleAnimationTime()에서 호출
    /// </summary>
    private void CheckNextAttack()
    {
        // 현재 재생 중인 애니메이션이 공격 리스트의 현재 공격과 일치하면 다음 공격 가능
        if (playerController.animator.GetCurrentAnimatorStateInfo(0).IsName(attackList[attackCount]) == true)
        {
            goNextAttack = true;
        }
    }

    /// <summary>
    /// 공격 콜라이더를 토글합니다.
    /// 사용처: OnEnter(), OnExit()에서 호출
    /// </summary>
    private void AttackCollider()
    {
        // 공격 콜라이더 토글
        attackPoint.enabled = !attackPoint.enabled;
    }

    
    /// <summary>
    /// 물리 업데이트 주기마다 호출됩니다.
    /// 사용처: 상태 머신의 StateFixedUpdate()에서 호출
    /// </summary>
    public void OnFixedUpdate()
    {
        AttackDeceleration();
    }
    
    /// <summary>
    /// 공격 시 감속을 처리합니다.
    /// 사용처: OnFixedUpdate()에서 호출
    /// </summary>
    private void AttackDeceleration()
    {
        // x축 속도를 점진적으로 0으로 감소
        playerController.frameVelocity.x = Mathf.MoveTowards(
            playerController.frameVelocity.x, 
            0, 
            attackDeceleration * Time.fixedDeltaTime);
    }


    /// <summary>
    /// 상태가 종료될 때 호출됩니다.
    /// 사용처: 상태 머신에서 상태 전환 시 호출
    /// </summary>
    public void OnExit()
    {
        // 공격 콜라이더 비활성화
        AttackCollider();
        // 다음 공격 플래그 초기화
        goNextAttack = false;
    }

    /// <summary>
    /// 현재 상태의 이름을 반환합니다.
    /// </summary>
    /// <returns>상태 이름</returns>
    public string StateName()
    {
        return "Attack";
    }
}
