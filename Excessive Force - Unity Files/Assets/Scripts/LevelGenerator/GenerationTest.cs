using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationTest : MonoBehaviour
{
    [Header("Level Generation (Rooms + Corridors)")]
    public GameObject startRoom;
    public List<GameObject> roomPrefabs;
    public List<GameObject> corridorPrefabs;

    [Header("Level Generation (Details)")]
    public GameObject spawnPoint;
    public GameObject endPoint;
    public GameObject hexSphere;

    [Header("Level Generation (Info)")]
    public static int iterations = 75;
    private List<GameObject> spawnedSections = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateLevel());
    }


    /*
    ====================================================================================================
    Entry Point
    ====================================================================================================
    */
    private IEnumerator GenerateLevel()
    {
        // Room Before Info
        GameObject roomBefore;
        RoomData beforeData;

        // Room After Info
        GameObject roomAfter;
        RoomData afterData;

        // Generating Start Room
        roomBefore = Instantiate(startRoom, this.transform);
        roomAfter = roomBefore;
        spawnedSections.Add(roomBefore);
        beforeData = roomBefore.GetComponent<RoomData>();
        afterData = beforeData;

        // Adding Spawn Point
        GameObject spawn = Instantiate(spawnPoint, this.transform);

        // Generating Rest Of The Level
        int i = iterations;
        bool spawnRoom = false;
        while (i > 0 || spawnRoom)
        {
            // Selecting Connection Point
            List<ConnectionPoint> possibleNextPoints = beforeData.GetPossibleConnectionPoints();
            ConnectionPoint connection = possibleNextPoints[Random.Range(0, possibleNextPoints.Count)];

            bool successful = false;
            while (!successful)
            {
                // Spawning Next Room
                if (spawnRoom)
                {
                    roomAfter = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)], this.transform);
                }
                else
                {
                    roomAfter = Instantiate(corridorPrefabs[Random.Range(0, corridorPrefabs.Count)], this.transform);
                }
                afterData = roomAfter.GetComponent<RoomData>();

                // Aligning Next Room
                roomAfter.transform.position = connection.transform.position + (connection.transform.forward * Vector3.Distance(roomAfter.transform.position, afterData.tileConnections[0].transform.position));
                roomAfter.transform.rotation = connection.transform.rotation;

                // Checking For Intersections
                yield return new WaitForSeconds(0.1f);
                if (!afterData.isColliding)
                {
                    successful = true;
                }
                else
                {
                    DestroyImmediate(roomAfter);
                }
            }
            
            // Iterate
            connection.connectedTile = afterData;
            afterData.tileConnections[0].connectedTile = beforeData;

            // Setting Data For Next Iteration
            spawnRoom = !spawnRoom;
            spawnedSections.Add(roomAfter);
            roomBefore = roomAfter;
            beforeData = afterData;

            // Iterating
            i--;
        }

        // Setting Reset Plane Below The Level
        SetDeathPlane();

        // Adding End Point
        GameObject end = GameObject.Instantiate(endPoint, this.transform);
        end.transform.position = spawnedSections[spawnedSections.Count - 1].transform.position;

        // Finishing Touches
        DetailLevel();

        foreach (GameObject g in spawnedSections)
        {
            g.GetComponent<RoomData>().CanCollisionCheck(false);
        }
    }


    /*
    ====================================================================================================
    Detail + Other Additions Generation
    ====================================================================================================
    */
    private void DetailLevel()
    {
        // Getting Centre Point
        Vector3 centrePoint = Vector3.zero;
        foreach (GameObject g in spawnedSections)
        {
            centrePoint += g.transform.position;
        }
        centrePoint /= spawnedSections.Count;

        // Setting HexSphere Position & Size
        // Setting Position
        hexSphere.transform.position = centrePoint;
        // Setting Scale
        Vector3 furthestPoint = spawnedSections[0].transform.position;
        foreach (GameObject g in spawnedSections)
        {
            if (Vector3.Distance(g.transform.position, hexSphere.transform.position) > Vector3.Distance(furthestPoint, hexSphere.transform.position))
            {
                furthestPoint = g.transform.position;
            }
        }
        float scale = Vector3.Distance(hexSphere.transform.position, furthestPoint);
        scale /= 2;
        hexSphere.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void SetDeathPlane()
    {
        // Finding Room At Lowest Point
        float lowestPoint = 0;
        foreach (GameObject g in spawnedSections)
        {
            if (g.transform.position.y < lowestPoint)
            {
                lowestPoint = g.transform.position.y;
            }
        }
        Debug.Log("Lowest Point: " + lowestPoint);

        // Setting Respawn Plane
        RespawnManager.Instance.respawnPoint = (int)(lowestPoint - 8);
        Debug.Log("Respawn Point: " + RespawnManager.Instance.respawnPoint);
    }
}

public enum SymmetryType
{
    NONE,
    BILATERAL,
    TRILATERAL
}