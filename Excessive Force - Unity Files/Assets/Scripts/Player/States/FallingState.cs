using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : PlayerState
{
    override public void StartState(PlayerController thePlayer)
    {
        // Setting the new animation state
        thePlayer.animController.SetBool("Grounded", false);
        thePlayer.animController.SetTrigger("Falling");
    }

    override public void UpdateState(PlayerController thePlayer)
    {
        // State Changes
        if (thePlayer.IsGrounded())
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                thePlayer.ChangeState(thePlayer.playerMoving);
            }
            else
            {
                thePlayer.ChangeState(thePlayer.playerIdle);
            }
        }

        // Weapon Controls
        if (Input.GetMouseButtonDown(0))
        {
            thePlayer.playerWeapon.Fire();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            thePlayer.playerWeapon.Reload();
        }
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        // State Movement
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            thePlayer.MovePlayer(thePlayer.airAccel);
        }
    }
}
