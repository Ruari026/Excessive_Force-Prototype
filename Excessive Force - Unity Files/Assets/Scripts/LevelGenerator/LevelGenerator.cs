
// Generation Algorithm Sourced From
// https://gamedevelopment.tutsplus.com/tutorials/bake-your-own-3d-dungeons-with-procedural-recipes--gamedev-14360

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> roomPrefabs;
    public GameObject startingRoom;

    public uint iterations = 5;

    private List<GameObject> spawnedRooms = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Remove Previously Loaded Rooms
            for (int i = 0; i < spawnedRooms.Count; i++)
            {
                Destroy(spawnedRooms[i]);
            }
            spawnedRooms = new List<GameObject>();

            // Starts New Level Generation
            GenerateLevel(this.startingRoom, this.roomPrefabs, this.iterations);
        }
    }

    private void GenerateLevel(GameObject startingRoom, List<GameObject> roomPrefabs, uint iterations)
    {
        GameObject start = Instantiate(startingRoom, this.transform);
        List<Transform> pendingExits = start.GetComponent<RoomData>().connectionPoints;

        while (iterations > 0)
        {
            List<Transform> newExits = new List<Transform>();

            foreach (Transform pendingExit in pendingExits)
            {

            }

            iterations--;
        }
    }

    private void MatchExits(Transform oldExit, Transform newExit)
    {

    }

    private void Azimuth(Vector3 vector)
    {

    }
}
