using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointScript : MonoBehaviour
{
    public void MoveToNextLevel()
    {
        GenerationTest.iterations *= 2;

        SceneTransitionController stc = SceneTransitionController.Instance;
        stc.ChangeScene("LevelGenerationTest", true);
    }
}
