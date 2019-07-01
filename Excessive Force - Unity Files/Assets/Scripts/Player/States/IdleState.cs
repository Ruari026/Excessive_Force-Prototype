using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    override public void StartState(PlayerController thePlayer)
    {
        // Setting Animation State
        thePlayer.animController.SetBool("Moving", false);
        thePlayer.animController.SetBool("Grounded", true);
    }

    override public void UpdateState(PlayerController thePlayer)
    {
        // State Changes
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            thePlayer.ChangeState(thePlayer.playerMoving);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            thePlayer.ChangeState(thePlayer.playerJumping);
        }
        else if (!thePlayer.IsGrounded())
        {
            thePlayer.ChangeState(thePlayer.playerFalling);
        }
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        Vector2 velocity = Vector2.zero;
        thePlayer.theRB.velocity = new Vector3(velocity.x, thePlayer.theRB.velocity.y, velocity.y);
    }
}
