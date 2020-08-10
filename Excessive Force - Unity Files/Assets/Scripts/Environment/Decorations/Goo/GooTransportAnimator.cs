using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooTransportAnimator : MonoBehaviour
{
    // Goo
    [SerializeField] private GameObject pipeContents;
    private Material blobMaterial;

    //Noise Generation
    private Texture2D noiseTexture;
    private float seed = 0;
    [SerializeField] private float noiseSpeed;
    private float noiseTime;

    // Start is called before the first frame update
    void Start()
    {
        blobMaterial = pipeContents.GetComponent<MeshRenderer>().material;

        seed = Random.Range(0, 1.0f);

        noiseTexture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        noiseTexture.hideFlags = HideFlags.DontSave;
        noiseTexture.wrapMode = TextureWrapMode.Repeat;
        noiseTexture.filterMode = FilterMode.Point;
    
        blobMaterial.SetTexture("_NoiseTexture", noiseTexture);
    }

    // Update is called once per frame
    void Update()
    {
        // Rim Shader Handling
        blobMaterial.SetVector("_CameraPos", Camera.main.transform.position);

        // Noise Handling
        noiseTime += (Time.deltaTime * noiseSpeed);

        for (int y = 0; y < noiseTexture.height; y++)
        {
            for (int x = 0; x < noiseTexture.width; x++)
            {
                float xCoord = (((float)x / 16) + (noiseTime / 16));
                float yCoord = ((float)y / 16) + seed;

                float value = Mathf.PerlinNoise(xCoord, yCoord);
                noiseTexture.SetPixel(x, y, new Color(value, value, value));
            }
        }
        noiseTexture.Apply();
    }
}
