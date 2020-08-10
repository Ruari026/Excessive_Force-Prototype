using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CableGenerator))]
public class CableGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get End Point"))
        {
            CableGenerator script = (CableGenerator)target;
            script.FindEndPoint();
        }

        if (GUILayout.Button("Generate Spline"))
        {
            CableGenerator script = (CableGenerator)target;
            script.GenerateSpline();
        }

        if (GUILayout.Button("Generate Cable"))
        {
            CableGenerator script = (CableGenerator)target;
            script.GenerateMesh();
        }
    }
}