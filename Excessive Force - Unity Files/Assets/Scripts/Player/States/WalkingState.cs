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

        // State Animations
        Vector3 movementDirection = thePlayer.transform.forward;
        if (Input.GetAxis("Vertical") > 0)
        {
            movementDirection += thePlayer.transform.right * Input.GetAxis("Horizontal");
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            movementDirection -= thePlayer.transform.right * Input.GetAxis("Horizontal");
        }
        thePlayer.modelHips.transform.rotation = Quaternion.Lerp(thePlayer.modelHips.transform.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), Time.deltaTime * thePlayer.hipRotationSpeed);

        // Weapon Controls
        thePlayer.playerWeapon.GunInput();
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        // State Movement
        thePlayer.MovePlayer(thePlayer.groundAccel);
    }
}
