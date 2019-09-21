using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomData))]
public class RoomDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Combine Meshes"))
        {
            RoomData script = (RoomData)target;
            script.CombineMesh();
        }

        if (GUILayout.Button("Generate Colliders"))
        {
            RoomData script = (RoomData)target;
            script.GenerateColliders();
        }
    }
}
