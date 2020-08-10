using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGameplayState : CameraState
{
    private float yaw = 0;
    private float pitch = 0;
    public Vector2 pitchLimits = new Vector2(-30, 60);
    public Vector2 zoomLimits = new Vector2(-2, -10);
    public float moveSpeed;

    public CameraGameplayState()
    {
        cameraOffset = new Vector3(0.5f, 1.1f, -4.0f);
    }

    public override void StartState(CameraController theCamera)
    {
        // Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Setting new camera offset
        theCamera.gameCamera.transform.localPosition = cameraOffset;
    }

    public override void UpdateState(CameraController theCamera)
    {
        // Orbiting the camera around the player
        // Affecting Rotation On The Y Axis
        yaw += Input.GetAxis("Mouse X") * 90 * Time.deltaTime;
        // Affecting Rotation On The X Axis
        pitch -= Input.GetAxis("Mouse Y") * 90 * Time.deltaTime;
        if (pitch < pitchLimits.x)
        {
            pitch = pitchLimits.x;
        }
        else if (pitch > pitchLimits.y)
        {
            pitch = pitchLimits.y;
        }
        theCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0);

        // Setting Camera Distance From The Player
        float zoomPercentage = (pitch - pitchLimits.x) / (pitchLimits.y - pitchLimits.x);
        Vector3 camPos = theCamera.gameCamera.transform.localPosition;
        camPos.z = ((zoomLimits.y - zoomLimits.x) * zoomPercentage) + zoomLimits.x;

        //Overriding Camera Zoom if too close to an object
        Ray ray = new Ray
        {
            origin = theCamera.followTarget.transform.position,
            direction = theCamera.followTarget.transform.forward * -1
        };
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            float distance = Vector3.Distance(theCamera.followTarget.transform.position, hit.point) * -1;
            if (distance > camPos.z)
            {
                camPos.z = distance;
            }
        }
        theCamera.gameCamera.transform.localPosition = Vector3.Lerp(theCamera.gameCamera.transform.localPosition, camPos, Time.deltaTime * moveSpeed);
    }
}
