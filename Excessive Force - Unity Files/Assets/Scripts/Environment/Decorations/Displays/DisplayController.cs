using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayController : MonoBehaviour
{
    [Header("Glitching Material Handling")]
    public bool canGlitch = true;
    [SerializeField, Range(0, 1)]
    private float maxIntensity = 0.4f;
    [SerializeField]
    private float maxTimeBetweenGlitches = 3.0f;
    [SerializeField]
    private List<GlitchingScreenController> displayScreens;
    private bool isGlitching = false;
    
    [Header("Screen Material Handling")]
    [SerializeField] private Vector2 offsetDirection = Vector2.one;
    [SerializeField] private float animSpeed = 0.2f;
    private float animTime = 0;
    [SerializeField] private MeshRenderer screenObject;
    private Material screenMaterial;
    
    [Header("Text Rendering Generation")]
    [SerializeField]
    private Camera sourceCamera;
    private RenderTexture outputTexture;

    void Start()
    {
        // Instancing Materials
        screenMaterial = screenObject.material;

        offsetDirection.Normalize();

        // Setting Up Text Rendering-
        if (sourceCamera)
        {
            outputTexture = new RenderTexture(512, 512, 24, RenderTextureFormat.Default);
            sourceCamera.targetTexture = outputTexture;

            sourceCamera.orthographicSize = (0.5f * sourceCamera.transform.lossyScale.y);

            outputTexture.Create();
        }

        // Starting Screen Animnation
        StartCoroutine(GlitchAnim(1.0f, 0.0f));
    }

    void Update()
    {
        if (!isGlitching || !canGlitch)
        {
            // Runs The Regular Screen Scrolling Animation
            animTime += (Time.deltaTime * animSpeed);
            if (animTime > 10.0f)
            {
                animTime -= 10.0f;
            }

            Vector2 newOffset = offsetDirection * animTime;
            screenMaterial.SetTextureOffset("_MainTex", newOffset);
        }
        else
        {
            float f = Random.value;
            if (f > Mathf.Lerp(0.9f, 0.5f, maxIntensity))
            {
                foreach (GlitchingScreenController gsc in displayScreens)
                {
                    gsc.GlitchScreen();
                }

                // Jumps The Scrolling Background Ahead
                animTime += f * 5.0f;
                Vector2 newOffset = offsetDirection * animTime;
                screenMaterial.SetTextureOffset("_MainTex", newOffset);
            }
        }
    }


    /*
    ====================================================================================================
    Screen Image Changing
    ====================================================================================================
    */
    public void SetScreenImage(Texture2D newImage)
    {
        for (int i = 0; i < displayScreens.Count; i++)
        {
            SetScreenImage(newImage, i);
        }
    }
    
    public void SetScreenImage(Texture2D newImage, int screenToChange)
    {
        displayScreens[screenToChange].ChangeImage(newImage);
    }


    /*
    ====================================================================================================
    Screen Glitching
    ====================================================================================================
    */
    private IEnumerator GlitchAnim(float animTime, float intensity)
    {
        foreach (GlitchingScreenController gsc in displayScreens)
        {
            gsc.UpdateIntensity(intensity);
        }
        

        if (intensity > 0.0f)
        {
            isGlitching = true;
        }
        else
        {
            isGlitching = false;
        }

        yield return new WaitForSeconds(animTime);

        if (intensity == 0.0f && canGlitch)
        {
            StartCoroutine(GlitchAnim(Random.Range(0.0f, 1.0f), Random.Range(0.0f, maxIntensity)));
        }
        else
        {
            StartCoroutine(GlitchAnim(Random.Range(0.0f, maxTimeBetweenGlitches), 0.0f));
        }
    }


    /*
    ====================================================================================================
    Text Rendering
    ====================================================================================================
    */
    public void ChangeText()
    {
        RenderTexture.active = outputTexture;
        Texture2D newTexture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        newTexture.ReadPixels(new Rect(0, 0, outputTexture.width, outputTexture.height), 0, 0);
        newTexture.Apply();

        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                Color c = newTexture.GetPixel(x, y);
                if (c == sourceCamera.backgroundColor)
                {
                    c = new Color(1, 1, 1, 0);
                    newTexture.SetPixel(x, y, c);
                }
            }
        }
        SetScreenImage(newTexture);
    }
}
