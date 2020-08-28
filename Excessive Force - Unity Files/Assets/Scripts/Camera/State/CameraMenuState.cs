using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuState : CameraState
{
    public Quaternion targetRotation;

    public CameraMenuState()
    {

    }

    public override void StartState(CameraController theCamera)
    {
        // Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Setup
        cameraOffset = new Vector3(0.75f, 1.3f, -1.98f);
        targetRotation = Quaternion.Euler(theCamera.followTarget.transform.eulerAngles + new Vector3(0, 200, 0));
    }

    public override void UpdateState(CameraController theCamera)
    {
        // Camera Mount Position
        theCamera.transform.position = Vector3.Lerp(theCamera.transform.position, theCamera.followTarget.transform.position, (Time.deltaTime * 2));

        // Camera Mount Rotation
        theCamera.transform.rotation = Quaternion.Lerp(theCamera.transform.rotation, targetRotation, (Time.deltaTime * 2));

        // Camera Offset
        theCamera.gameCamera.transform.localPosition = Vector3.Lerp(theCamera.gameCamera.transform.localPosition, cameraOffset, (Time.deltaTime * 2));
    }
}