using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    private void Update()
    {
        float fps = 1 / Time.deltaTime;
        Debug.Log("FPS: " + fps);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GenerationTest.iterations = Random.Range(25, 75);
            Debug.Log("Next Level Size: " + GenerationTest.iterations);

            SceneManager.LoadScene("LevelGenerationTest");
        }
    }
}
