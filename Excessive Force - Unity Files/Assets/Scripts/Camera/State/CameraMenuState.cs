using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuState : CameraState
{
    public CameraMenuState()
    {
        cameraOffset = new Vector3(-0.31f, 0.2f, -1.98f);
    }

    public override void StartState(CameraController theCamera)
    {
        // Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Setting new camera offset
        theCamera.gameCamera.transform.localPosition = cameraOffset;
    }

    public override void UpdateState(CameraController theCamera)
    {

    }
}