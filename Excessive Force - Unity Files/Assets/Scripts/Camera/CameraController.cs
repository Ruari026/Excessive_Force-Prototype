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
    public Vector2 zoomLimits = new Vector2(-2, -10);

    public static bool disabled = false;

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

        if (!disabled)
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
            this.transform.eulerAngles = new Vector3(pitch, yaw, 0);

            // Setting Camera Distance From The Player
            float zoomPercentage = (pitch - pitchLimits.x) / (pitchLimits.y - pitchLimits.x);
            Vector3 camPos = theCamera.transform.localPosition;
            camPos.z = ((zoomLimits.y - zoomLimits.x) * zoomPercentage) + zoomLimits.x;

            //Overriding Camera Zoom if too close to an object
            Ray ray = new Ray
            {
                origin = followTarget.transform.position,
                direction = followTarget.transform.forward * -1
            };
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                float distance = Vector3.Distance(followTarget.transform.position, hit.point) * -1;
                if (distance > camPos.z)
                {
                    camPos.z = distance; 
                }
            }
            theCamera.transform.localPosition = Vector3.Lerp(theCamera.transform.localPosition, camPos, Time.deltaTime * 10);
        }
    }
}
