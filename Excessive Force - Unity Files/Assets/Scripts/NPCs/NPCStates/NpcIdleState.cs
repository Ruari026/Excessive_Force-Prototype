using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcState
{
    private EyeAnimations theEyes;

    override public void StartState(NpcController theNPC)
    {
        theEyes = theNPC.GetComponent<EyeAnimations>();
    }

    override public void UpdateState(NpcController theNPC)
    {
        // Checks if the player is in a cone of view
        PlayerController thePlayer = GameObject.FindObjectOfType<PlayerController>();
        Vector3 playerPos = thePlayer.modelSpine.transform.position;

        Vector3 npcForward = theEyes.lookMount.transform.forward;
        Vector3 playerDir = playerPos - theNPC.transform.position;
        float angle = Vector3.Angle(npcForward, playerDir);

        if (angle < 60)
        {
            // Eye tracking player position
            theEyes.lookTarget.transform.position = playerPos;
        }
        else
        {
            // Reset Eye Tracking
            theEyes.lookTarget.transform.localPosition = Vector3.zero;
        }
    }

    override public void FixedUpdateState(NpcController theNPC)
    {

    }
}
