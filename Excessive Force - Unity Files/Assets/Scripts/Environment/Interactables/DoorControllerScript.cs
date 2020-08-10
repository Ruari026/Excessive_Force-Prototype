using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControllerScript : MonoBehaviour
{
    [SerializeField] private bool canOpen = true;

    [Header("Door Opening Animation")]
    [Range(0, 1)]
    [SerializeField] private float doorAnim;
    [SerializeField] private float animDistance;
    [SerializeField] List<GameObject> doorCenterPivots = new List<GameObject>();

    private GameObject targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen)
        {
            // Getting Distance To Player
            float distance = Vector3.Distance(this.transform.position, targetPlayer.transform.position);
            if (distance > animDistance)
            {
                doorAnim += Time.deltaTime;
                if (doorAnim > 1)
                {
                    doorAnim = 1;
                }
            }
            else
            {
                doorAnim -= Time.deltaTime;
                if (doorAnim < 0)
                {
                    doorAnim = 0;
                }
            }

            // Handling Door Anim
            float t = (1 - (1 - doorAnim) * (1 - doorAnim) * (1 - doorAnim) * (1 - doorAnim));
            foreach (GameObject g in doorCenterPivots)
            {
                Vector3 newScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

                g.transform.localScale = newScale;
            }
        }
    }
}