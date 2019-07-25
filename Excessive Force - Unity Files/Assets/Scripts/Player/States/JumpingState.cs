using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public float jumpDurationThreshold = 0.25f;
    private float jumpDuration = 0;

    override public void StartState(PlayerController thePlayer)
    {
        jumpDuration = 0;

        // Setting the new animation state
        thePlayer.animController.SetTrigger("Jumping");
        thePlayer.animController.SetBool("Grounded", false);
    }

    override public void UpdateState(PlayerController thePlayer)
    {
        // Controlling Jump Time
        if (Input.GetKey(KeyCode.Space))
        {
            jumpDuration += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpDuration = jumpDurationThreshold;
        }

        // State Changes
        if (thePlayer.IsGrounded() && jumpDuration >= jumpDurationThreshold)
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
        if (jumpDuration < jumpDurationThreshold)
        {
            thePlayer.theRB.velocity = new Vector3(thePlayer.theRB.velocity.x, thePlayer.jumpSpeed, thePlayer.theRB.velocity.z);
        }
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            thePlayer.MovePlayer(thePlayer.airAccel);
        }
    }
}
