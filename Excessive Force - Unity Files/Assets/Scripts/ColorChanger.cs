using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    //Nice Color Palletes Information
    private NiceColorPalettes ncp = new NiceColorPalettes();
    private int currentPalette = 0;

    //Player Information
    public Material[] playerMaterials = new Material[5];

    //UI Information
    public Image[] materialColors = new Image[5];
    public InputField nameField;

    // Start is called before the first frame update
    void Start()
    {
        //Loading the color palette information from file
        string filePath = Application.streamingAssetsPath + "/NiceColorPalettes/100.json";
        LoadFromJsonFile(filePath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentPalette = Random.Range(0, ncp.palettes.Count);

            //Getting the current palette's color information
            for (int i = 0; i < 5; i++)
            {
                Color newColor;
                if (ColorUtility.TryParseHtmlString(ncp.palettes[currentPalette].palette[i], out newColor))
                {
                    playerMaterials[i].color = newColor;
                    materialColors[i].color = newColor;
                }
            }

            //Getting the current palette's name
            nameField.text = ncp.palettes[currentPalette].name;
        }
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
        ncp.palettes[currentPalette].name = nameField.text;

        //Saving to json file
        string jsonString = JsonUtility.ToJson(ncp, true);
        File.WriteAllText(filePath, jsonString);
    }
}



[System.Serializable]
public class NiceColorPalettes
{
    public List<Palette> palettes = new List<Palette>();
}

[System.Serializable]
public class Palette
{
    public string name = "";
    public List<string> palette = new List<string>();
}
