using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    private Vector3 hipRotation;

    override public void StartState(PlayerController thePlayer)
    {
        // Setting Animation State
        thePlayer.animController.SetBool("Moving", false);
        thePlayer.animController.SetBool("Grounded", true);

        hipRotation = thePlayer.transform.eulerAngles;
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
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            thePlayer.ChangeState(thePlayer.playerDodging);
        }
        else if (!thePlayer.IsGrounded())
        {
            thePlayer.ChangeState(thePlayer.playerFalling);
        }

        
        //Checking Difference between hip rotation & spine rotation
        float difference = Quaternion.Angle(thePlayer.modelHips.transform.rotation, thePlayer.modelSpine.transform.rotation);
        if (difference > 60)
        {
            Quaternion spineRot = thePlayer.modelSpine.transform.rotation;

            thePlayer.modelHips.transform.rotation = Quaternion.Lerp(thePlayer.modelHips.transform.rotation, new Quaternion(0, spineRot.y, 0, spineRot.w), Time.deltaTime);
            hipRotation = thePlayer.modelHips.transform.eulerAngles;

            thePlayer.modelSpine.transform.rotation = spineRot;
        }
        else
        {
            thePlayer.modelHips.transform.eulerAngles = hipRotation;
        }
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        // State Movement
        Vector2 velocity = Vector2.zero;
        thePlayer.theRB.velocity = new Vector3(velocity.x, thePlayer.theRB.velocity.y, velocity.y);
    }
}
