using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraState
{
    public Vector3 cameraOffset;

    virtual public void StartState(CameraController theCamera)
    {

    }

    virtual public void UpdateState(CameraController theCamera)
    {

    }
}


public enum CameraStates
{
    STATE_GAMEPLAY,
    STATE_MENU
}