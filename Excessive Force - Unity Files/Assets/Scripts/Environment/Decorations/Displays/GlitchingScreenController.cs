using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchingScreenController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer glitchingObject;
    private Material glitchingMaterial;

    private Texture2D _noiseTexture;

    private void Start()
    {
        // Setting Up Noise Texture
        _noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);
        _noiseTexture.hideFlags = HideFlags.DontSave;
        _noiseTexture.wrapMode = TextureWrapMode.Clamp;
        _noiseTexture.filterMode = FilterMode.Point;
        
        glitchingMaterial = glitchingObject.material;
        glitchingMaterial.SetTexture("_NoiseTex", _noiseTexture);
    }


    /*
    ====================================================================================================
    Screen Image
    ====================================================================================================
    */
    public void ChangeImage(Texture2D newImage)
    {
        if (glitchingMaterial == null)
        {
            glitchingMaterial = glitchingObject.material;
        }
        glitchingMaterial.SetTexture("_MainTex", newImage);
    }


    /*
    ====================================================================================================
    Glitching
    ====================================================================================================
    */
    public void GlitchScreen()
    {
        UpdateNoiseTexture();
        
    }

    public void UpdateIntensity(float newIntensity)
    {
        if (glitchingMaterial != null)
        {
            glitchingMaterial.SetFloat("_Intensity", newIntensity);
        }
    }
    
    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1.0f);
    }

    void UpdateNoiseTexture()
    {
        var color = RandomColor();

        for (var y = 0; y < _noiseTexture.height; y++)
        {
            for (var x = 0; x < _noiseTexture.width; x++)
            {
                if (Random.value > 0.89f) color = RandomColor();
                _noiseTexture.SetPixel(x, y, color);
            }
        }

        _noiseTexture.Apply();
    }
}
