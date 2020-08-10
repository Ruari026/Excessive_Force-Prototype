using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    private float currentRotation = 0;
    public float rotationSpeed = 30;

    private void Start()
    {
        currentRotation = this.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        if (currentRotation > 360)
        {
            currentRotation -= 360;
        }
        this.transform.eulerAngles = new Vector3(0, currentRotation, 0);
    }
}
