using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcState
{
    virtual public void StartState(NpcController theNPC)
    {

    }

    virtual public void UpdateState(NpcController theNPC)
    {

    }

    virtual public void FixedUpdateState(NpcController theNPC)
    {

    }

    virtual public void ExitState(NpcController theNPC)
    {

    }
}


public enum NpcStates
{
    STATE_IDLE = 0,
    STATE_DIALOGUE = 1
}