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
       
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
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
