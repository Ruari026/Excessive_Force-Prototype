using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    private static SceneTransitionController _instance;
    public static SceneTransitionController Instance
    {
        get
        {
            return _instance;
        }
    }

    private Animator theAnimController;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        theAnimController = this.GetComponent<Animator>();
    }


    /*
    ====================================================================================================
    Scene Starting
    ====================================================================================================
    */
    public void FinishLoading()
    {
        if (theAnimController.GetCurrentAnimatorStateInfo(0).IsName("Loading"))
        {
            theAnimController.SetTrigger("Finished");
        }
    }


    /*
    ====================================================================================================
    Scene Changing
    ====================================================================================================
    */
    public void ChangeScene(string newScene, bool isLoading)
    {
        theAnimController.SetTrigger("Started");
        StartCoroutine(DelaySceneChange(newScene));
    }
    private IEnumerator DelaySceneChange(string newScene)
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(newScene);
    }
}
