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
    public static int iterations = 50;
    private List<GameObject> spawnedSections = new List<GameObject>();

    public delegate void EventGenerationFinished();
    public static event EventGenerationFinished OnEventGenetationFinished;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateLevel());
    }


    /*
    ====================================================================================================
    Main Body Generation
    ====================================================================================================
    */
    private IEnumerator GenerateLevel()
    {
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        Debug.Log("Generation Started (" + GenerationTest.iterations +   " Iterations): " + stopWatch.Elapsed);

        // Generating Start Room
        GameObject newRoom = Instantiate(startRoom, this.transform);
        RoomData newData = newRoom.GetComponent<RoomData>();
        spawnedSections.Add(newRoom);

        // Generating Rest Of The Level
        int i = iterations;
        bool spawnRoom = false;
        while (i > 0 || spawnRoom)
        {
            Debug.Log("Spawning New Room");

            bool placementSuccessful = false;

            // Selecting Connection Point From Last Room In Spawned Rooms
            List<ConnectionPoint> possibleNextPoints = spawnedSections[spawnedSections.Count - 1].GetComponent<RoomData>().GetPossibleConnectionPoints();
            Debug.Log(possibleNextPoints.Count);

            while(!placementSuccessful && possibleNextPoints.Count > 0)
            {
                Debug.Log("Picking Connection Point To Spawn Off");

                // Selecting Spawn Point
                int cp = Random.Range(0, possibleNextPoints.Count);
                ConnectionPoint connection = possibleNextPoints[cp];
                possibleNextPoints.RemoveAt(cp);

                // Starting List Of Spawnable Objects
                List<GameObject> prefabsLeftToTry = new List<GameObject>();
                if (spawnRoom)
                {
                    // Spawning A Room
                    prefabsLeftToTry = new List<GameObject>(roomPrefabs);
                }
                else
                {
                    // Spawning A Corridor
                    prefabsLeftToTry = new List<GameObject>(corridorPrefabs);
                }

                // Generate Room
                while (!placementSuccessful && prefabsLeftToTry.Count > 0)
                {
                    // Spawning Next Room
                    int roomToSpawn = Random.Range(0, prefabsLeftToTry.Count);
                    newRoom = Instantiate(prefabsLeftToTry[roomToSpawn], this.transform);
                    prefabsLeftToTry.RemoveAt(roomToSpawn);
                    newData = newRoom.GetComponent<RoomData>();

                    // Aligning Next Room
                    newRoom.transform.position = connection.transform.position + (connection.transform.forward * Vector3.Distance(newRoom.transform.position, newData.tileConnections[0].transform.position));
                    newRoom.transform.rotation = connection.transform.rotation;

                    // Giving Time For CollisionCheck
                    yield return new WaitForSeconds(0.01f);
                    if (!newData.isColliding)
                    {
                        placementSuccessful = true;
                    }
                    else
                    {
                        Debug.Log("Spawned Room Colliding, Removing New Room");
                        DestroyImmediate(newRoom);
                    }
                }

                if (placementSuccessful)
                {
                    Debug.Log("Room Spawned Sucessfully: " + newRoom.gameObject.name);

                    // Iterate
                    connection.connectedTile = newData;
                    newData.tileConnections[0].connectedTile = spawnedSections[spawnedSections.Count - 1].GetComponent<RoomData>();

                    // Setting Data For Next Iteration
                    spawnRoom = !spawnRoom;
                    spawnedSections.Add(newRoom);

                    // Iterating
                    i--;
                }
            }

            if (!placementSuccessful)
            {
                Debug.Log("No Spawn Point To Build Off");

                // Step Back A Stage
                DestroyImmediate(spawnedSections[spawnedSections.Count - 1]);
                spawnedSections.RemoveAt(spawnedSections.Count - 1);

                spawnRoom = !spawnRoom;

                // Adding to remaining iterations to ensure the level length is accurate to intended iteration count
                i++;
            }
        }

        // Setting Spawned Level To Be Playable
        foreach (GameObject g in spawnedSections)
        {
            g.GetComponent<RoomData>().CanCollisionCheck(false);
        }

        DetailLevel();
        SetDeathPlane();

        if (OnEventGenetationFinished != null)
        {
            OnEventGenetationFinished.Invoke();
        }
        SceneTransitionController stc = SceneTransitionController.Instance;
        stc.FinishLoading();

        // Generation Finished
        stopWatch.Stop();
        Debug.Log("Generation Finished: " + stopWatch.Elapsed);
    }


    /*
    ====================================================================================================
    Finalising Level Details
    ====================================================================================================
    */
    private void DetailLevel()
    {
        // Getting Centre Point
        Vector3 centerPoint = Vector3.zero;
        foreach (GameObject g in spawnedSections)
        {
            centerPoint += g.transform.position;
        }
        centerPoint /= spawnedSections.Count;

        // Setting HexSphere Position & Size
        // Setting Position
        hexSphere.transform.position = centerPoint;
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

        // Adding Start Point
        GameObject levelStart = Instantiate(spawnPoint);
        levelStart.transform.position = spawnedSections[0].transform.position;

        // Adding End Point
        GameObject levelEnd = Instantiate(endPoint);
        levelEnd.transform.position = spawnedSections[spawnedSections.Count - 1].transform.position;
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