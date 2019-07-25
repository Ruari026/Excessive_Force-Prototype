using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    //Full List Of Palettes
    private NiceColorPalettes ncp = new NiceColorPalettes();
    //Currently Selected Palette
    private Palette currentPalette;
    private int paletteNumber = 0;

    //Player Information
    public Material[] playerMaterials = new Material[5];

    //UI Information
    public Image[] materialColors = new Image[5];
    public InputField nameField;

    //Texture Generation
    public Texture2D textureSprite;
    public float darkenAmount = 0.6f;


    // Start is called before the first frame update
    void Start()
    {
        //Loading the color palette information from file
        string filePath = Application.streamingAssetsPath + "/NiceColorPalettes/100.json";
        LoadFromJsonFile(filePath);

        LoadPaletteInformation();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*
    ====================================================================================================
    Json Save And Load Handling
    ====================================================================================================
    */
    public void LoadFromJsonFile(string fileName)
    {
        //Loading from json file
        string jsonString = File.ReadAllText(fileName);
        ncp = JsonUtility.FromJson<NiceColorPalettes>(jsonString);
    }

    public void SaveToJsonFile()
    {
        string filePath = Application.streamingAssetsPath + "/NiceColorPalettes/100.json";

        //Updating current palette information
        ncp.palettes[paletteNumber].name = nameField.text;

        //Saving to json file
        string jsonString = JsonUtility.ToJson(ncp, true);
        File.WriteAllText(filePath, jsonString);
    }

    public void LoadPaletteInformation()
    {
        currentPalette = ncp.GetPalette(paletteNumber);

        //Getting the current palette's color information
        for (int i = 0; i < 5; i++)
        {
            if (ColorUtility.TryParseHtmlString(currentPalette.palette[i], out Color newColor))
            {
                playerMaterials[i].color = newColor;
                materialColors[i].color = newColor;
            }
        }

        //Getting the current palette's name
        nameField.text = currentPalette.name;
    }

    private void SavePaletteInformation()
    {
        ncp.palettes[paletteNumber] = currentPalette;
    }


    /*
    ====================================================================================================
    Handling Changing Palettes Via UI
    ====================================================================================================
    */
    public void IncreaseCurrentPalette()
    {
        paletteNumber++;

        if (paletteNumber == ncp.palettes.Count)
        {
            paletteNumber = 0;
        }

        LoadPaletteInformation();
    }

    public void DecreaseCurrentPalette()
    {
        paletteNumber--;

        if (paletteNumber < 0)
        {
            paletteNumber = (ncp.palettes.Count - 1);
        }

        LoadPaletteInformation();
    }


    /*
    ====================================================================================================
    Creating Texture From Current Palette
    ====================================================================================================
    */
    public void GenerateTexture()
    {
        Color[] previousColors = textureSprite.GetPixels();

        //Converting to 2d Array
        Color[,] colors = new Color[512, 512];
        int x = 0;
        int y = 0;
        for (int i = 0; i < previousColors.Length; i++)
        {
            colors[x, y] = previousColors[i];
            x++;
            if (x == 512)
            {
                x = 0;
                y++;
            }
        }

        Rect[] colorRects = new Rect[5];
        colorRects[0] = new Rect(0, 0, 128, 512);
        colorRects[1] = new Rect(128, 0, 128, 512);
        colorRects[2] = new Rect(256, 0, 128, 512);
        colorRects[3] = new Rect(384, 256, 128, 256);
        colorRects[4] = new Rect(384, 0, 128, 256);

        for (int r = 0; r < 5; r++)
        {
            Rect currentRect = colorRects[r];
            for (int j = (int)currentRect.y; j < (int)currentRect.height + (int)currentRect.y; j++)
            {
                for (int i = (int)currentRect.x; i < (int)currentRect.width + (int)currentRect.x; i++)
                {
                    ColorUtility.TryParseHtmlString(currentPalette.palette[r], out Color baseColor);
                    Color darkened = new Color
                    {
                        r = baseColor.r * darkenAmount,
                        g = baseColor.g * darkenAmount,
                        b = baseColor.b * darkenAmount,
                        a = 255
                    };

                    float lerpPoint = (j / colorRects[r].height);

                    Color newColor = new Color
                    {
                        r = Mathf.Lerp(baseColor.r, darkened.r, lerpPoint),
                        g = Mathf.Lerp(baseColor.g, darkened.g, lerpPoint),
                        b = Mathf.Lerp(baseColor.b, darkened.b, lerpPoint),
                        a = 255
                    };

                    colors[i, j] = newColor;
                }
            }
        }

        //Converting Back to 1D Array
        Color[] newColors = new Color[previousColors.Length];
        int z = 0;
        for (y = 0; y < 512; y ++)
        {
            for (x = 0; x < 512; x ++)
            {
                newColors[z] = colors[x, y];
                z++;
            }
        }

        textureSprite.SetPixels(newColors);
        textureSprite.Apply();
        File.WriteAllBytes(AssetDatabase.GetAssetPath(textureSprite), textureSprite.EncodeToPNG());
    }
}



[System.Serializable]
public class NiceColorPalettes
{
    public List<Palette> palettes = new List<Palette>();

    public Palette GetPalette(int paletteNumber)
    {
        Palette newPalette = palettes[paletteNumber];

        return newPalette;
    }
}

[System.Serializable]
public class Palette
{
    public string name = "";
    public List<string> palette = new List<string>();
    public string description = "";
}
