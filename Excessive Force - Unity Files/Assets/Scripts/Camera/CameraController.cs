using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followTarget;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Following the player character
        this.transform.position = followTarget.transform.position;

        //Orbiting the camera around the player
        Vector3 currentRotation = this.transform.eulerAngles;
        currentRotation.y += Input.GetAxis("Mouse X") * 90 * Time.deltaTime;
        this.transform.eulerAngles = currentRotation;
    }
}
