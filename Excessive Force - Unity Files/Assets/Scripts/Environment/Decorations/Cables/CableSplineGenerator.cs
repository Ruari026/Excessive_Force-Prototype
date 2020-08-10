using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CableSplineGenerator : MonoBehaviour
{
    [Header("Spline Generation Details")]
    public CinemachineSmoothPath thePath;

    [SerializeField] private Transform cableOrigin;
    [SerializeField] private Transform cableEnd;
    [SerializeField, Range(0, 1)] private float indentAmount;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void GenerateSpline()
    {
        float tangentPower = 1;
        do
        {
            GeneratePoints(tangentPower);
            tangentPower -= 0.1f;
        }
        while (DoPointsCollide());
    }
    
    public void FindEndPoint()
    {

    }

    void GeneratePoints(float tangentReduction)
    {
        List<CinemachineSmoothPath.Waypoint> newWaypoints = new List<CinemachineSmoothPath.Waypoint>();
        float distance = Vector3.Distance(cableOrigin.position, cableEnd.position);

        // Start Point
        CinemachineSmoothPath.Waypoint startPoint = new CinemachineSmoothPath.Waypoint();
        startPoint.position = cableOrigin.localPosition;

        // End Point
        CinemachineSmoothPath.Waypoint endPoint = new CinemachineSmoothPath.Waypoint();
        endPoint.position = cableEnd.localPosition;

        // Indented Points
        CinemachineSmoothPath.Waypoint indentedStart = new CinemachineSmoothPath.Waypoint();
        indentedStart.position = new Vector3
        {
            x = cableEnd.localPosition.x * (indentAmount / 2),
            y = cableEnd.localPosition.y * (indentAmount / 2),
            z = cableEnd.localPosition.z * (indentAmount)
        };

        CinemachineSmoothPath.Waypoint indentedEnd = new CinemachineSmoothPath.Waypoint();
        indentedEnd.position = new Vector3
        {
            x = cableEnd.localPosition.x * (1 - (indentAmount / 2)),
            y = cableEnd.localPosition.y * (1 - (indentAmount / 2)),
            z = cableEnd.localPosition.z * (1 - indentAmount)
        };

        // Finalising Points
        newWaypoints.Add(startPoint);
        newWaypoints.Add(indentedStart);
        newWaypoints.Add(indentedEnd);
        newWaypoints.Add(endPoint);

        thePath.m_Waypoints = newWaypoints.ToArray();
    }

    bool DoPointsCollide()
    {
        int iterations = Mathf.FloorToInt(thePath.PathLength);
        iterations--;

        for (int i = 0; i < iterations; i++)
        {
            Vector3 rayStart = thePath.EvaluatePositionAtUnit(i, CinemachinePathBase.PositionUnits.Distance);
            Vector3 rayEnd = thePath.EvaluatePositionAtUnit(i + 1, CinemachinePathBase.PositionUnits.Distance);
        }

        return false;
    }
}
