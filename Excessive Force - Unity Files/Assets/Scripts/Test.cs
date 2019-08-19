using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    public MeshCollider room1;
    public MeshCollider room2;

    // Update is called once per frame
    void Update()
    {
        if (room1.bounds.Intersects(room2.bounds))
        {
            Debug.Log("Doot Doot I Did A Toot");
        }
    }
}
