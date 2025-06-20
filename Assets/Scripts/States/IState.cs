using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태 패턴에서 사용되는 상태 인터페이스입니다.
/// 모든 상태 클래스는 이 인터페이스를 구현해야 합니다.
/// </summary>
public interface IState
{
    /// <summary>
    /// 상태가 시작될 때 호출됩니다.
    /// 사용처: 상태 머신에서 상태 전환 시 호출
    /// </summary>
    void OnEnter();

    /// <summary>
    /// 매 프레임 호출됩니다.
    /// 사용처: 상태별 업데이트 로직 처리
    /// </summary>
    void OnUpdate();
    
    /// <summary>
    /// 물리 업데이트 주기마다 호출됩니다.
    /// 사용처: 물리 관련 로직 처리
    /// </summary>
    void OnFixedUpdate();
    
    /// <summary>
    /// 상태가 종료될 때 호출됩니다.
    /// 사용처: 상태 종료 시 정리 작업 수행
    /// </summary>
    void OnExit();

    /// <summary>
    /// 현재 상태의 이름을 반환합니다.
    /// 사용처: 디버깅 및 상태 확인용
    /// </summary>
    /// <returns>상태 이름 문자열</returns>
    string StateName();
}
