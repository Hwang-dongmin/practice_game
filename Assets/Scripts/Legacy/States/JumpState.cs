using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpState : IState
{
    //For reference
    private PlayerController playerController;

    //For reusing ForceToRun()
    private IState runningState;

    //Check Jump of Fall
    private bool isJump;

    //»ý¼ºÀÚ
    public JumpState(PlayerController playerController) {
        this.playerController = playerController;  
    }



    public void OnEnter()
    {
        if (CheckPlayerJump())
        {
            JumpFromGround();
        }

        playerController.animator.SetBool("isJumping", true);
    }


    private bool CheckPlayerJump()
    {
        if (playerController.isGround == true)
        {
            //Jumped from ground
            playerController.isGround = false;
            isJump= true;
            return true;
        }
        else
        {
            //Just fall down
            isJump= false;
            return false;
        }
    }
    private void JumpFromGround()
    {
        playerController.frameVelocity.y = playerController.jumpPower;

    }
    public void OnUpdate()
    {
        //Revise the animation of Jump according to the velocity of y
        ReviseJumpAnimation();
        //Check whether player landed. if so, trans to Idle
        TransToIdle();

        playerController.ChangeSpriteDirection();
    }

    private void ReviseJumpAnimation()
    {
        //Need to Change
        
         
        
    }

    private void TransToIdle()
    {
        if (playerController.isGround && playerController.rigid.velocity.y <= 0f)
        {
            isJump = false;
            playerController.animator.SetBool("isJumping", false);
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.idleState);
        }
    }

    public void OnFixedUpdate()
    {
        playerController.ForceToRunning();
        HandleGravity();


    }

    private void HandleGravity()
    {
        //if JumpingState
        //change velocity.y from 'current' to max in a speed of fallAcceleration
        playerController.frameVelocity.y = Mathf.MoveTowards(playerController.frameVelocity.y, -playerController.maxFallSpeed, playerController.fallAcceleraction * Time.fixedDeltaTime);
        
    }

    




    public void OnExit()
    {

    }
}
