using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomData : MonoBehaviour
{
    [Header("Connections")]
    public List<ConnectionPoint> tileConnections = new List<ConnectionPoint>();
    
    [Header("Symmetry")]
    public SymmetryType roomSymmetry;
    public Transform symmetryPoint;

    [Header("Collision Detection")]
    public bool collisionCheck = false;
    public bool isColliding = false;
    public List<GameObject> RoomTiles = new List<GameObject>();


    /*
    ====================================================================================================
    Passing Information
    ====================================================================================================
    */
    public List<ConnectionPoint> GetPossibleConnectionPoints()
    {
        List<ConnectionPoint> viablePoints = new List<ConnectionPoint>();
        foreach (ConnectionPoint c in tileConnections)
        {
            if (c.connectedTile == null)
            {
                viablePoints.Add(c);
            }
        }

        return viablePoints;
    }


    /*
    ====================================================================================================
    Inspector Methods
    ====================================================================================================
    */
    public void CombineMesh()
    {
        Mesh combinedMesh = new Mesh();
        CombineInstance[] combine = new CombineInstance[RoomTiles.Count];

        for (int i = 0; i < RoomTiles.Count; i++)
        {
            combine[i].mesh = RoomTiles[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = RoomTiles[i].transform.localToWorldMatrix;
        }
        combinedMesh.CombineMeshes(combine);

        string filePath = "Assets/";
        filePath += this.gameObject.name.ToString();
        filePath += "Mesh.asset";
        AssetDatabase.CreateAsset(combinedMesh, filePath);
    }

    public void GenerateColliders()
    {
        Transform colliders = this.transform.GetChild(this.transform.childCount - 1);
        for (int i = 0; i < colliders.childCount; i++)
        {
            DestroyImmediate(colliders.GetChild(i));
        }

        foreach (GameObject g in RoomTiles)
        {
            if (g.tag == "FullFloor" || g.tag == "FullRamp")
            {
                // Creating Primitive
                GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                c.transform.parent = this.transform.GetChild(this.transform.childCount - 1).transform;

                //Setting Transform
                // Position
                c.transform.position = new Vector3
                {
                    x = g.transform.position.x,
                    y = g.transform.position.y + 2.5f,
                    z = g.transform.position.z
                };
                // Scale
                c.transform.localScale = new Vector3(3, 3.5f, 3);

                // Removing Renderer
                DestroyImmediate(c.GetComponent<MeshRenderer>());
                DestroyImmediate(c.GetComponent<MeshFilter>());
            }
        }
    }


    /*
    ====================================================================================================
    Spawn Collisions
    ====================================================================================================
    */
    private void OnCollisionEnter(Collision collision)
    {
        collisionCheck = true;

        if (collision.gameObject.tag == "Room" || collision.gameObject.tag == "Corridor")
        {
            isColliding = true;
        }
    }

    public void CanCollisionCheck(bool canCheck)
    {
        Rigidbody theRB = this.GetComponent<Rigidbody>();
        MeshCollider theMC = this.GetComponent<MeshCollider>();

        if (canCheck)
        {
            Transform colliders = this.transform.GetChild(this.transform.childCount - 1);
            for (int i = 0; i < colliders.childCount; i++)
            {
                colliders.GetChild(i).gameObject.SetActive(true);
            }

            theRB.isKinematic = false;
            theMC.convex = true;

            theMC.enabled = false;
        }
        else
        {
            theMC.enabled = true;

            theRB.isKinematic = true;
            theMC.convex = false;

            Transform colliders = this.transform.GetChild(this.transform.childCount - 1);
            for (int i = 0; i < colliders.childCount; i++)
            {
                colliders.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    /*
    ====================================================================================================
    Debugging
    ====================================================================================================
    */
    private void OnDrawGizmosSelected()
    {
        // Drawing Connection Points
        foreach (ConnectionPoint c in tileConnections)
        {
#if UNITY_EDITOR
            if (c != null)
            {
                Transform t = c.gameObject.transform;

                Gizmos.color = Color.red;
                Vector3 startX = t.transform.position + (t.transform.right);
                Vector3 endX = t.transform.position - (t.transform.right);
                Gizmos.DrawLine(startX, endX);

                Gizmos.color = Color.green;
                Vector3 startY = t.transform.position + (t.transform.up);
                Vector3 endY = t.transform.position;
                Gizmos.DrawLine(startY, endY);

                Gizmos.color = Color.blue;
                Vector3 startZ = t.transform.position + (t.transform.forward);
                Vector3 endZ = t.transform.position;
                Gizmos.DrawLine(startZ, endZ);

                GUI.color = Color.magenta;
                Handles.Label((t.transform.position + (t.transform.forward * 0.5f) + (t.transform.up * 0.5f)), c.connectionType);
            }
#endif
        }

        // Drawing Room Symmetry
        if (symmetryPoint != null && roomSymmetry != SymmetryType.NONE)
        {
            int divisions = 0;
            switch (roomSymmetry)
            {
                case (SymmetryType.BILATERAL):
                    {
                        divisions = 2;
                    }
                    break;

                case (SymmetryType.TRILATERAL):
                    {
                        divisions = 3;
                    }
                    break;
            }

            for (int i = 0; i < divisions; i++)
            {
                Vector3 rayStart = symmetryPoint.position + (Vector3.up * 0.2f);

                float rotationAmount = (360 / divisions);
                Vector3 rayEnd = Quaternion.AngleAxis((rotationAmount * i) + symmetryPoint.transform.eulerAngles.y, Vector3.up) * (Vector3.forward);
                rayEnd = (rayEnd * 1000) + rayStart;

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(rayStart, rayEnd);
            }
        }
    }
}

public enum SymmetryType
{
    NONE,
    BILATERAL,
    TRILATERAL
}