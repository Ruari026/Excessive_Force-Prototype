using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraPixelationEffect : MonoBehaviour
{
    [SerializeField]
    private Material material;

    public bool canPixelate = false;
    [SerializeField]
    private float pixelationAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPixelate)
        {
            Vector2 screenDimentions = new Vector2(Screen.width, Screen.height);
            screenDimentions /= pixelationAmount;

            material.SetFloat("_Columns", screenDimentions.x);
            material.SetFloat("_Rows", screenDimentions.y);
        }
        else
        {
            material.SetFloat("_Columns", Screen.width);
            material.SetFloat("_Rows", Screen.height);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, material);
    }
}
