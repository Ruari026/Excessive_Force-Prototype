using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public List<Transform> connectionPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        foreach (Transform t in connectionPoints)
        {
            Gizmos.color = Color.red;
            Vector3 startX = t.transform.position + t.transform.right;
            Vector3 endX = t.transform.position + t.transform.right * -1;
            Gizmos.DrawLine(startX, endX);

            Gizmos.color = Color.green;
            Vector3 startY = t.transform.position + t.transform.up;
            Vector3 endY = t.transform.position;
            Gizmos.DrawLine(startY, endY);

            Gizmos.color = Color.blue;
            Vector3 startZ = t.transform.position + t.transform.forward;
            Vector3 endZ = t.transform.position;
            Gizmos.DrawLine(startZ, endZ);
        }
    }
}
