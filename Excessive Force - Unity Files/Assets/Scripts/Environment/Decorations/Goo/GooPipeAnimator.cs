using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooPipeAnimator : MonoBehaviour
{
    [Header("Goo Decoration Parts")]
    [SerializeField] private GameObject goostream;
    [SerializeField] private GameObject gooPuddle;
    [SerializeField] private GameObject streamStart;
    [SerializeField] private GameObject streamEnd;

    private Material streamGooMaterial;
    private Material puddleGooMaterial;

    [Header("Goo Decoration Animation")]
    [SerializeField] private float streamSpeed;
    private float streamAnimTime;
    [SerializeField] private float puddleSpeed;
    private float puddleAnimTime;

    [Header("Unique Noise Handling")]
    private float seed = 0;
    private Texture2D noiseTexture;

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(0, 1.0f);

        streamGooMaterial = goostream.GetComponent<MeshRenderer>().material;
        puddleGooMaterial = gooPuddle.GetComponent<MeshRenderer>().material;

        noiseTexture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        noiseTexture.hideFlags = HideFlags.DontSave;
        noiseTexture.wrapMode = TextureWrapMode.Repeat;
        noiseTexture.filterMode = FilterMode.Point;

        streamGooMaterial.SetTexture("_NoiseTexture", noiseTexture);
        puddleGooMaterial.SetTexture("_NoiseTexture", noiseTexture);

        streamGooMaterial.SetVector("_GooOrigin", streamStart.transform.position);
        streamGooMaterial.SetVector("_GooFloor", streamEnd.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Animating Goo Parts Based On Time
        // Pipe Stream
        streamAnimTime += (Time.deltaTime * streamSpeed);
        Vector3 newStreamRot = goostream.transform.localEulerAngles;
        newStreamRot.z = streamAnimTime;
        goostream.transform.localEulerAngles = newStreamRot;
        streamGooMaterial.SetVector("_CameraPos", Camera.main.transform.position);

        // Pipe Puddle
        puddleAnimTime += (Time.deltaTime * puddleSpeed);
        puddleGooMaterial.SetFloat("_Offset", puddleAnimTime);
        puddleGooMaterial.SetVector("_CameraPos", Camera.main.transform.position);

        // Adding noise to each Part
        for (int y = 0; y < noiseTexture.height; y++)
        {
            for (int x = 0; x < noiseTexture.width; x++)
            {
                float xCoord = (((float)x / 32) + (puddleAnimTime / 32));
                float yCoord = ((float)y / 32) + seed;

                float value = Mathf.PerlinNoise(xCoord, yCoord);
                noiseTexture.SetPixel(x, y, new Color(value, value, value));
            }
        }
        noiseTexture.Apply();
    }
}
