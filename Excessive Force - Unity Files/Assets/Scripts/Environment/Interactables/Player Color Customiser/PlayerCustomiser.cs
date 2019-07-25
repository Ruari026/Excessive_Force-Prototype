using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomiser : InteractableObject
{
    public GameObject model;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        Vector3 newRotation = model.transform.eulerAngles;
        newRotation.y += 45 * Time.deltaTime;
        if (newRotation.y > 360)
        {
            newRotation.y -= 360;
        }
        model.transform.eulerAngles = newRotation;
    }
}
