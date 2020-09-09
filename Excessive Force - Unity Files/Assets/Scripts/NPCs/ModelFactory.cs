using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelFactory : MonoBehaviour
{
    public ModelPartTypes factoryType;
    public List<GameObject> modelPrefabs;

    public GameObject GetGameObject()
    {
        int i = Random.Range(0, modelPrefabs.Count);
        return GameObject.Instantiate(modelPrefabs[i]);
    }
}

public enum ModelPartTypes
{ 
    ARMS,
    BACKPACK,
    CHEST,
    FEET,
    HANDS,
    HEAD,
    SHOULDERS,
    WAIST
}