using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject theCamera;
    public GameObject followTarget;

    private float yaw = 0;
    private float pitch = 0;
    public Vector2 pitchLimits = new Vector2(-30, 60);
    public Vector2 zoomLimits = new Vector2(0, -8);

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Following the player character
        this.transform.position = followTarget.transform.position;

        //Orbiting the camera around the player
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
        // Camera Zoom
        float zoomPercentage = (pitch - pitchLimits.x) / (pitchLimits.y - pitchLimits.x);
        Vector3 camPos = theCamera.transform.localPosition;
        camPos.z = ((zoomLimits.y - zoomLimits.x) * zoomPercentage) + zoomLimits.x;
        theCamera.transform.localPosition = camPos;

        this.transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }
}
