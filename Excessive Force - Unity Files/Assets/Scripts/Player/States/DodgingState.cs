using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingState : PlayerState
{
    Vector2 dodgeDirection;

    override public void StartState(PlayerController thePlayer)
    {
        thePlayer.animController.SetTrigger("Rolling");

        dodgeDirection = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        dodgeDirection.Normalize();

        thePlayer.StartCoroutine(WaitTilDodgeEnd(thePlayer));

        thePlayer.followCameraRotation = false;

        Vector3 turnDirection = Vector3.zero;
        turnDirection += thePlayer.transform.forward * dodgeDirection.x;
        turnDirection += thePlayer.transform.right * dodgeDirection.y;
        thePlayer.transform.rotation = Quaternion.LookRotation(turnDirection, Vector3.up);
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        thePlayer.MovePlayer(thePlayer.groundAccel * 10, new Vector2(0,1));
    }

    private IEnumerator WaitTilDodgeEnd(PlayerController thePlayer)
    {
        yield return new WaitForSeconds(0.85f);

        thePlayer.followCameraRotation = true;
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
