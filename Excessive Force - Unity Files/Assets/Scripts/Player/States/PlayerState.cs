using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    virtual public void StartState(PlayerController thePlayer)
    {

    }

    virtual public void UpdateState(PlayerController thePlayer)
    {

    }

    virtual public void FixedUpdateState(PlayerController thePlayer)
    {

    }

    virtual public void CheckCollisionState(PlayerController thePlayer, Collision collision)
    {

    }
}


public enum PlayerStates
{
    STATE_IDLE,
    STATE_MENU,
    STATE_MOVING,
    STATE_JUMPING,
    STATE_FALLING,
    STATE_DISABLED
}