using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : PlayerState
{
    override public void StartState(PlayerController thePlayer)
    {
        // Setting the new animation state
        thePlayer.animController.SetBool("Moving", true);
        thePlayer.animController.SetBool("Grounded", true);
    }

    override public void UpdateState(PlayerController thePlayer)
    {
        // State Changes
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            thePlayer.ChangeState(thePlayer.playerIdle);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
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
        thePlayer.MovePlayer(thePlayer.groundAccel);
    }
}
