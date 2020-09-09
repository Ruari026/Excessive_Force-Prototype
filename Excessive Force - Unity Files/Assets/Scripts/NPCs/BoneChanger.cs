using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneChanger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer theRenderer;
    [SerializeField] private Transform[] bones;

    // Start is called before the first frame update
    void Start()
    {
        bones = theRenderer.bones;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
