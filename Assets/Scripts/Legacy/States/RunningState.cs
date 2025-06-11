using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : IState
{
    //For reference
    private PlayerController playerController;

    public RunningState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void OnEnter()
    {
        playerController.animator.SetBool("isRunning", true);

    }
    public void OnUpdate()
    {
        TransToIdle();
        ChangeSpriteDirection();
        
    }

    private void TransToIdle() {
        //Revising speed when stop
        if (playerController.inputDirection.x == 0)
        {
            playerController.rigid.velocity = new Vector2(playerController.rigid.velocity.x * 0.5f, playerController.rigid.velocity.y);
            playerController.stateMachine.StateTransitionTo(playerController.stateMachine.idleState);
        }

    }

    private void ChangeSpriteDirection()
    {
        // change the direction of character depending on movement(keypressed)
        if (Input.GetButtonDown("Horizontal"))
        {
            playerController.spriteRenderer.flipX = playerController.inputDirection.x == -1;
        }
    }


    public void OnFixedUpdate()
    {
        
    }

    private void ForceToRunning()
    {
        //Running logic
        //Add Force to Player Using the inputDirection of PlayerController
        playerController.rigid.AddForce(playerController.inputDirection, ForceMode2D.Impulse);

        //Restrict the Speed using maxSpeed
        if (Mathf.Abs(playerController.rigid.velocity.x) > playerController.maxSpeed)
        {
            playerController.rigid.velocity = new Vector2(playerController.maxSpeed * Mathf.Sign(playerController.rigid.velocity.x), playerController.rigid.velocity.y);
        }
    }

    public void OnExit()
    {
        playerController.animator.SetBool("isRunning", false);
    }


}
