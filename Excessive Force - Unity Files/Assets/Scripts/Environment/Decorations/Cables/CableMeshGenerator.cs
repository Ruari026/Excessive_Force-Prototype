using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CableMeshGenerator : MonoBehaviour
{
    [Header("Cable Generation Details")]
    [SerializeField] private MeshFilter theMesh;
    [SerializeField] private MeshRenderer theRenderer;
    [SerializeField] private int cableSides = 16;
    [SerializeField] private int cableResolution = 1;
    [SerializeField] private float cableRadius = 1;
    [SerializeField] private Material cableMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void GenerateCable(CinemachinePathBase thePath)
    {
        // Setup
        List<Vector3> newVertexes = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        List<int> newTris = new List<int>();

        float cableLength = thePath.PathLength;

        // Mesh Generation
        // Start Circle
        Vector3 pointCenter = (thePath.EvaluatePosition(0));
        pointCenter = (pointCenter - theMesh.transform.position);
        pointCenter = (Quaternion.Inverse(theMesh.transform.rotation) * pointCenter);

        Quaternion pointOrientation = (thePath.EvaluateOrientation(0));
        pointOrientation = (pointOrientation * Quaternion.Inverse(theMesh.transform.rotation));

        for (int i = 0; i <= cableSides; i++)
        {
            // Vertex Pos
            float angle = (((Mathf.PI * 2) / cableSides) * i);
            Vector3 newPoint = new Vector3((Mathf.Sin(angle)), (Mathf.Cos(angle)), 0);
            newPoint = newPoint * cableRadius;

            newPoint = pointOrientation * newPoint;
            newPoint = newPoint + pointCenter;

            // Vertex UVs
            Vector2 newUV = new Vector2(0, i);

            newVertexes.Add(newPoint);
            newUVs.Add(newUV);
        }

        // Handling Center of Spline
        float pathStep = (float)thePath.PathLength / cableResolution;
        for (int j = 1; j <= cableResolution; j++)
        {
            // Adding next ring of vertexes
            pointCenter = (thePath.EvaluatePositionAtUnit(j * pathStep, CinemachinePathBase.PositionUnits.Distance));
            pointCenter = (pointCenter - theMesh.transform.position);
            pointCenter = (Quaternion.Inverse(theMesh.transform.rotation) * pointCenter);

            pointOrientation = (thePath.EvaluateOrientationAtUnit(j * pathStep, CinemachinePathBase.PositionUnits.Distance));
            pointOrientation = (Quaternion.Inverse(theMesh.transform.rotation) * pointOrientation);

            for (int i = 0; i <= cableSides; i++)
            {
                // Vertex Pos
                float angle = (((Mathf.PI * 2) / cableSides) * i);
                Vector3  newPoint = new Vector3((Mathf.Sin(angle)), (Mathf.Cos(angle)), 0);
                newPoint = newPoint * cableRadius;
                
                newPoint = pointOrientation * newPoint;
                newPoint = newPoint + pointCenter;

                // Vertex UVs
                Vector2 newUV = new Vector2(j * pathStep, i);

                newVertexes.Add(newPoint);
                newUVs.Add(newUV);
            }

            // Connecting rings
            int currentTriStart = (j - 1) * (cableSides + 1);
            int previousTriStart = j * (cableSides + 1);

            for (int i = 0; i <= cableSides; i++)
            {
                // Gets the 4 relevant vertexes
                int vert1, vert2, vert3, vert4;
                vert1 = previousTriStart + i;
                vert3 = currentTriStart + i;

                if (i == (cableSides))
                {
                    vert2 = previousTriStart;
                    vert4 = currentTriStart;
                }
                else
                {
                    vert2 = previousTriStart + i + 1;
                    vert4 = currentTriStart + i + 1;
                }


                // Connects 2 tris together
                newTris.Add(vert1);
                newTris.Add(vert2);
                newTris.Add(vert4);

                newTris.Add(vert1);
                newTris.Add(vert4);
                newTris.Add(vert3);
            }
        }

        // End of Spline


        // Mesh Assignment
        Mesh newMesh = new Mesh();

        newMesh.vertices = newVertexes.ToArray();
        newMesh.uv = newUVs.ToArray();
        newMesh.triangles = newTris.ToArray();

        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();

        theMesh.mesh = newMesh;
        theRenderer.material = cableMat;
    }
}
