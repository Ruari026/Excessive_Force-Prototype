using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledState : PlayerState
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
        
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        // State Movement
        Vector2 velocity = Vector2.zero;
        thePlayer.theRB.velocity = new Vector3(velocity.x, thePlayer.theRB.velocity.y, velocity.y);
    }
}
