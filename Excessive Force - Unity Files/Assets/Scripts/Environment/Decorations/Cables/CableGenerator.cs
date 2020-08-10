using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CableGenerator : MonoBehaviour
{
    public bool connected = false;

    [SerializeField] private CableSplineGenerator theLine;
    [SerializeField] private List<CableMeshGenerator> theModels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindEndPoint()
    {

    }

    public void GenerateSpline()
    {
        theLine.GenerateSpline();
    }

    public void GenerateMesh()
    {
        foreach(CableMeshGenerator model in theModels)
        {
            model.GenerateCable(theLine.thePath);
        }
    }
}
