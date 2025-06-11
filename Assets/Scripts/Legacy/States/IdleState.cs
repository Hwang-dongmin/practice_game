using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class IdleState : IState
{
    //For reference
    private PlayerController playerController;

    //»ý¼ºÀÚ
    public IdleState( PlayerController playerController) {  this.playerController = playerController; }



    public void OnEnter()
    {
        //animator to idle
    }

    public void OnUpdate()
    {
        // Start running
        ChangeToRunning();
        //change animation to idle
        ChangeToStop();
        playerController.ChangeSpriteDirection();

        playerController.frameVelocity.y = 0;

        TransToJumpState();
        TransToAttackState();

        

    }



    public void OnFixedUpdate()
    {
        playerController.ForceToRunning();
        
    }

    // Handle Gravity Function
    /*
    private void HandleGravity()
    {
        // We dont need Gravity when it's in  IdleState.
        
        if (isGround == true && frameVelocity.y <= 0)
        {
            //if IdleState
            frameVelocity.y = 0f;
        }
        
    }
    */

    public void OnExit()
    {

    }

    public void TransToAttackState()
    {
        if (playerController.TryAttack())
        {
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.attackState);
        }
    }

    private void ChangeToStop()
    {
        //Revising speed when stop
        if (playerController.inputDirection.x == 0)
        {
            playerController.animator.SetBool("isRunning", false);
        }

    }

    private void ChangeToRunning()
    {
        // change the direction of character depending on movement(keypressed)
        if (playerController.inputDirection.x != 0)
        {
            //playerController.stateMachine.StateTransitionTo(playerController.stateMachine.runningState);
            playerController.animator.SetBool("isRunning", true);
        }
    }

    

    private void TransToJumpState() {
        if (playerController.isGround == false || playerController.TryJump())
        {
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.jumpState);
        }
    }

}
