using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    private void Update()
    {
        float fps = 1 / Time.deltaTime;
        //Debug.Log("FPS: " + fps);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneTransitionController stc = SceneTransitionController.Instance;
            stc.ChangeScene("MainMenu", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GenerationTest.iterations += 25;

            SceneTransitionController stc = SceneTransitionController.Instance;
            stc.ChangeScene("LevelGenerationTest", true);
        }
    }
}
