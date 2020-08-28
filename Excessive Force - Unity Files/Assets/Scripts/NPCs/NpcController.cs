using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    // The state the npc is currently in
    public NpcStates startState;
    private NpcState currentState;

    // All possible states for the npc to be in
    public NpcIdleState npcIdle;
    public NpcDialogueState npcDialogue;

    // Dialogue
    public string dialogueFileName;

    // Start is called before the first frame update
    void Start()
    {
        npcIdle = new NpcIdleState();
        npcDialogue = new NpcDialogueState();

        ChangeState(startState);
    }


    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    /*
    ====================================================================================================
    Handling State Changes
    ====================================================================================================
    */
    public void ChangeState(int newState)
    {
        ChangeState((NpcStates)newState);
    }

    public void ChangeState(NpcState newState)
    {
        //OnEventPlayerStateChange?.Invoke(newState);
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        this.currentState = newState;
        this.currentState.StartState(this);
    }

    public void ChangeState(NpcStates newState)
    {
        switch (newState)
        {
            case (NpcStates.STATE_IDLE):
                {
                    ChangeState(npcIdle);
                }
                break;

            case (NpcStates.STATE_DIALOGUE):
                {
                    ChangeState(npcDialogue);
                }
                break;
        }
    }

    public IEnumerator ChangeStateTemporary(NpcState tempState, float time)
    {
        NpcState stateBefore = this.currentState;
        ChangeState(tempState);

        yield return new WaitForSeconds(time);

        ChangeState(stateBefore);
    }
    public IEnumerator ChangeStateTemporary(NpcState tempState, NpcState newState, float time)
    {
        ChangeState(tempState);

        yield return new WaitForSeconds(time);

        ChangeState(newState);
    }
}
