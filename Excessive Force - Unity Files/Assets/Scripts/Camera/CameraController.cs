using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // The state the player is currently in
    public CameraStates startState;
    private CameraState currentState;

    // All possible states for the player to be in
    public CameraGameplayState cameraGameplay;
    public CameraMenuState cameraMenu;
    public CameraDialogueState cameraDialogue;

    // Camera Mount Info
    public GameObject gameCamera;
    public GameObject followTarget;

    // Start is called before the first frame update
    void Start()
    {
        cameraGameplay = new CameraGameplayState();
        cameraMenu = new CameraMenuState();
        cameraDialogue = new CameraDialogueState();

        ChangeState(startState);
    }


    /*
    ====================================================================================================
    Updating States
    ====================================================================================================
    */
    void Update()
    {
        // Following the player character
        this.transform.position = followTarget.transform.position;

        // State Handling
        currentState.UpdateState(this);
    }


    /*
    ====================================================================================================
    Handling State Changes
    ====================================================================================================
    */
    public void ChangeState(CameraState newState)
    {
        this.currentState = newState;
        this.currentState.StartState(this);
    }

    public void ChangeState(CameraStates newState)
    {
        switch (newState)
        {
            case (CameraStates.STATE_GAMEPLAY):
                {
                    ChangeState(cameraGameplay);
                }
                break;

            case (CameraStates.STATE_MENU):
                {
                    ChangeState(cameraMenu);
                }
                break;

            case (CameraStates.STATE_DIALOGUE):
                {
                    ChangeState(cameraDialogue);
                }
                break;
        }
    }

    public IEnumerator ChangeStateLerp(CameraState newState, float time)
    {
        float t = 0;
        Vector3 startOffset = gameCamera.transform.localPosition;
        Vector3 endOffset = newState.cameraOffset;

        while (t <= 1.0f)
        {
            t += (Time.deltaTime / time);

            Vector3 newOffset = Vector3.Lerp(startOffset, endOffset, t);
            gameCamera.transform.localPosition = newOffset;

            yield return null;
        }

        gameCamera.transform.localPosition = endOffset;
        ChangeState(newState);
    }

    public IEnumerator ChangeStateTemporary(CameraState tempState, float time)
    {
        CameraState stateBefore = this.currentState;
        ChangeState(tempState);

        yield return new WaitForSeconds(time);

        ChangeState(stateBefore);
    }
    public IEnumerator ChangeStateTemporary(CameraState tempState, CameraState newState, float time)
    {
        ChangeState(tempState);

        yield return new WaitForSeconds(time);

        ChangeState(newState);
    }
}
