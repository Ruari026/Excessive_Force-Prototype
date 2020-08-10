using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject menuCanvas;

    [Header("Play Menu")]
    public Text loadSizeText;
    public Slider loadSizeSlider;

    [Header("Player Details")]
    [SerializeField] private PlayerController thePlayer;
    [SerializeField] private CameraController theCamera;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneTransitionController stc = SceneTransitionController.Instance;
        stc?.FinishLoading();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*
    ====================================================================================================
    Play Options Menu
    ====================================================================================================
    */
    public void MoveToHub()
    {
        SceneTransitionController stc = SceneTransitionController.Instance;
        stc.ChangeScene("HUB", true);
    }

    public void MoveToGame()
    {
        // stc = SceneTransitionController.Instance;
        //stc.ChangeScene("LevelGenerationTest", true);

        menuCanvas.SetActive(false);

        StartCoroutine(MoveGameAnim(1));
    }

    private IEnumerator MoveGameAnim(float animTime)
    {
        // lerp camera to new transform
        float t = 0;
        Vector3 startPos = theCamera.transform.position;
        Quaternion startRot = theCamera.transform.rotation;

        while (t < 1.0)
        {
            t += (Time.deltaTime / animTime);

            Vector3 newPos = Vector3.Lerp(startPos, thePlayer.modelSpine.transform.position, t);
            Quaternion newRot = Quaternion.Lerp(startRot, thePlayer.transform.rotation, t);

            theCamera.transform.position = newPos;
            theCamera.transform.rotation = newRot;

            yield return null;
        }

        theCamera.transform.position = thePlayer.modelSpine.transform.position;
        theCamera.transform.rotation = thePlayer.transform.rotation;

        thePlayer.ChangeState(thePlayer.playerIdle);
        theCamera.ChangeState(theCamera.cameraGameplay);
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
    Quit Options Menu
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
