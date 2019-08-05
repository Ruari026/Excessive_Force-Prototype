using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class HexPositionSnapping : MonoBehaviour
{
    public static float xLock = 1.5f;
    public static float yLock = 0.5f;
    public static float zLock = 0.866f;

    // Update is called once per frame
    void Update()
    {
        if (Selection.Contains(this.gameObject))
        {
            // Snapping Position
            Vector3 snapPos = this.transform.position;

            snapPos.x = Mathf.Round((snapPos.x / xLock)) * xLock;
            snapPos.y = Mathf.Round((snapPos.y / yLock)) * yLock;
            snapPos.z = Mathf.Round((snapPos.z / zLock)) * zLock;

            this.transform.position = snapPos;
        }
    }
}
