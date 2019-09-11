using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject eyeLookTarget;

    [Header("Play Menu")]
    public Text loadSizeText;
    public Slider loadSizeSlider;

    [Header("Options Menu")]
    public Text volumeText;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneTransitionController stc = SceneTransitionController.Instance;
        stc.FinishLoading();
    }

    // Update is called once per frame
    void Update()
    {
        // Controlling Eye Target
        Vector3 mousePos = Input.mousePosition;

        // Clamping To Game Screen
        // X Axis
        if (mousePos.x < 0)
        {
            mousePos.x = 0;
        }
        else if (mousePos.x > Screen.width)
        {
            mousePos.x = Screen.width;
        }
        // Y Axis
        if (mousePos.y < 0)
        {
            mousePos.y = 0;
        }
        else if (mousePos.y > Screen.height)
        {
            mousePos.y = Screen.height;
        }
        // Z Axis
        mousePos.z = 1;

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        eyeLookTarget.transform.position = mousePos;
    }


    /*
    ====================================================================================================
    Play Menu
    ====================================================================================================
    */
    public void MoveToGame()
    {
        SceneTransitionController stc = SceneTransitionController.Instance;
        stc.ChangeScene("LevelGenerationTest", true);
    }

    public void SetLevelGenerationSize()
    {
        int newIterations = (int)loadSizeSlider.value;
        newIterations = (newIterations * 5) + 25;

        loadSizeText.text = "Level Size : " + newIterations;
        GenerationTest.iterations = newIterations;
    }


    /*
    ====================================================================================================
    Options Menu
    ====================================================================================================
    */
    public void SetGameVolume()
    {
        float newVolume = volumeSlider.value / 10;

        volumeText.text = "Master : " + newVolume.ToString("0.0");
        AudioListener.volume = newVolume;
    }


    /*
    ====================================================================================================
    Quit Confirmation
    ====================================================================================================
    */
    public void QuitGame()
    {
        Application.Quit();
    }


    /*
    ====================================================================================================
    Other Elements
    ====================================================================================================
    */
    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/Ruari026");
    }
}
