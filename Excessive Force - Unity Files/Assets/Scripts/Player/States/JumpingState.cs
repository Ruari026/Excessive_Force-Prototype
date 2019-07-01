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
        if (jumpDuration > jumpDurationThreshold)
        {
            thePlayer.ChangeState(thePlayer.playerFalling);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpDuration += Time.deltaTime;
        }
        else
        {
            thePlayer.ChangeState(thePlayer.playerFalling);
        }
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        if (jumpDuration < jumpDurationThreshold)
        {
            thePlayer.theRB.velocity = new Vector3(thePlayer.theRB.velocity.x, thePlayer.jumpSpeed, thePlayer.theRB.velocity.z);
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            thePlayer.MovePlayer(thePlayer.airAccel);
        }
    }

    public override void CheckCollisionState(PlayerController thePlayer, Collision collision)
    {
        // State Changes
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (Vector3.Angle(collision.GetContact(i).normal, Vector3.up) < 45)
            {
                if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                {
                    thePlayer.ChangeState(thePlayer.playerMoving);
                }
                else
                {
                    thePlayer.ChangeState(thePlayer.playerIdle);
                }
            }
        }
    }
}
