using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : PlayerState
{
    private Vector3 hipRotation;

    override public void StartState(PlayerController thePlayer)
    {
        // Setting Animation State
        thePlayer.animController.SetBool("Moving", false);
        thePlayer.animController.SetBool("Grounded", true);
        thePlayer.animController.SetBool("Menu", true);

        thePlayer.followCameraRotation = false;

        hipRotation = thePlayer.transform.eulerAngles;
    }

    override public void UpdateState(PlayerController thePlayer)
    {
        //Checking Difference between hip rotation & spine rotation
        float difference = Quaternion.Angle(thePlayer.modelHips.transform.rotation, thePlayer.modelSpine.transform.rotation);
        /*if (difference > 60)
        {
            Quaternion spineRot = thePlayer.modelSpine.transform.rotation;

            thePlayer.modelHips.transform.rotation = Quaternion.Lerp(thePlayer.modelHips.transform.rotation, new Quaternion(0, spineRot.y, 0, spineRot.w), Time.deltaTime);
            hipRotation = thePlayer.modelHips.transform.eulerAngles;

            thePlayer.modelSpine.transform.rotation = spineRot;
        }
        else
        {
            thePlayer.modelHips.transform.eulerAngles = hipRotation;
        }*/


        // Eye Animations
        Vector3 mousePos = Input.mousePosition;

        // Clamping To Game Screen
        // X Axis
        if (mousePos.x < 0)
        {
            mousePos.x = 0;
        }
        else if (mousePos.x > Screen.width)
        {
            mousePos.x = Screen.width;
        }
        // Y Axis
        if (mousePos.y < 0)
        {
            mousePos.y = 0;
        }
        else if (mousePos.y > Screen.height)
        {
            mousePos.y = Screen.height;
        }
        // Z Axis
        mousePos.z = 1;

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        thePlayer.eyeLookTarget.transform.position = mousePos;
    }

    public override void FixedUpdateState(PlayerController thePlayer)
    {
        // State Movement
        Vector2 velocity = Vector2.zero;
        thePlayer.theRB.velocity = new Vector3(velocity.x, thePlayer.theRB.velocity.y, velocity.y);
    }
}
