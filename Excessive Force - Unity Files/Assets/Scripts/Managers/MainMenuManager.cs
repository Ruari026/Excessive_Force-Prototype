using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject eyeLookTarget;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

        
        mousePos.z = 1;

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        eyeLookTarget.transform.position = mousePos;
    }


    /*
    ====================================================================================================
    Play  Game
    ====================================================================================================
    */
    public void LoadLevelGenerationTest()
    {
        SceneManager.LoadScene("LevelGenerationTest");
    }


    /*
    ====================================================================================================
    Options
    ====================================================================================================
    */


    /*
    ====================================================================================================
    Quitting Game
    ====================================================================================================
    */
    public void QuitGame()
    {

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
