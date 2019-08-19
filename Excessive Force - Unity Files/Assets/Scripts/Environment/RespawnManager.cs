using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager
{
    // Singleton Pattern
    private static RespawnManager _singleton = null;
    public static RespawnManager Instance
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = new RespawnManager();
            }
            return _singleton;
        }
    }

    public int respawnPoint = -10;
}
