using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeContentsAnimator : MonoBehaviour
{
    public bool canAnimate = true;

    [Header("Material Details")]
    [SerializeField] private Renderer targetMesh;
    private Material instancedMaterial;

    [Header("Animation Speeds")]
    [SerializeField] private float xSpeed = 1;
    [SerializeField] private float ySpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        instancedMaterial = targetMesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAnimate)
        {
            Vector2 newOffset = instancedMaterial.mainTextureOffset;

            newOffset = new Vector2
            {
                x = newOffset.x + (xSpeed * Time.deltaTime),
                y = newOffset.y + (ySpeed * Time.deltaTime)
            };

            instancedMaterial.mainTextureOffset = newOffset;
        }
    }
}
