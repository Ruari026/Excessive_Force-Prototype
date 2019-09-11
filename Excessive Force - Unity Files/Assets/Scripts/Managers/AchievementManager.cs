using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private static AchievementManager _instance;

    public static AchievementManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject g = new GameObject("Achievement Manager");
                _instance = g.AddComponent<AchievementManager>();
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
