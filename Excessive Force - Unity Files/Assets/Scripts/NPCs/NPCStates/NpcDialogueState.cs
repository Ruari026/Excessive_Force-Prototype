using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogueState : NpcState
{
    public DialogueTree dialogueTree;

    override public void StartState(NpcController theNPC)
    {
        // Locking the player in place
        PlayerController thePlayer = GameObject.FindObjectOfType<PlayerController>();
        thePlayer.ChangeState(PlayerStates.STATE_MENU);

        // Preventing Multiple Interactions
        theNPC.GetComponent<InteractableObject>().SetCanInteract(false);

        // Setting the camera to view both speakers
        CameraController theCamera = GameObject.FindObjectOfType<CameraController>();
        theCamera.ChangeState(CameraStates.STATE_DIALOGUE);

        // Starting Dialogue
        dialogueTree = DialogueParser.LoadFromFilePath(DialogueParser.defaultFilePath + theNPC.dialogueFileName + ".json");

        // Making Cameraview Independant of window size
        Camera dialogueCam = theNPC.GetComponentInChildren<Camera>();
        Vector2Int windowSize = new Vector2Int(Screen.width, Screen.height);

        DialogueCameraMasker cameraMask = dialogueCam.GetComponent<DialogueCameraMasker>();
        cameraMask.UpdateMask(windowSize);

        dialogueCam.enabled = true;

        // Showing Dialogue UI
        DialogueBoxController.Instance.StartDialogue(thePlayer, theNPC);
    }

    override public void UpdateState(NpcController theNPC)
    {

    }

    override public void ExitState(NpcController theNPC)
    {
        // Returning control to the player
        PlayerController thePlayer = GameObject.FindObjectOfType<PlayerController>();
        thePlayer.ChangeState(PlayerStates.STATE_IDLE);

        // Preventing Multiple Interactions
        theNPC.GetComponent<InteractableObject>().SetCanInteract(true);

        // Setting the camera to view both speakers
        CameraController theCamera = GameObject.FindObjectOfType<CameraController>();
        theCamera.ChangeState(CameraStates.STATE_GAMEPLAY);

        // Disabling unneeded camera
        Camera DialogueCam = theNPC.GetComponentInChildren<Camera>();
        DialogueCam.enabled = false;
    }
}
