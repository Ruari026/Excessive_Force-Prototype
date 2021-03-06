﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    [Header("Player Spawning")]
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    //Event Handling
    private void OnEnable()
    {
        GenerationTest.OnEventGenetationFinished += SpawnPlayer;
    }
    private void OnDisable()
    {
        GenerationTest.OnEventGenetationFinished -= SpawnPlayer;
    }


    /*
    ====================================================================================================
    Spawning
    ====================================================================================================
    */
    private void SpawnPlayer()
    {
        if (playerPrefab ==  null || cameraPrefab == null)
        {

        }
        else
        {
            GameObject newPlayer = Instantiate(playerPrefab);
            GameObject newCamera = Instantiate(cameraPrefab);

            PlayerController newPC = newPlayer.GetComponent<PlayerController>();
            CameraController newCC = newCamera.GetComponent<CameraController>();

            newPC.cameraMount = newCamera;
            newCC.followTarget = newPC.modelSpine;

            newPlayer.transform.position = this.transform.position + Vector3.up;
        }
    }


    /*
    ====================================================================================================
    Returning To Main Menu
    ====================================================================================================
    */
    public void ReturnToMainMenu()
    {
        SceneTransitionController stc = SceneTransitionController.Instance;
        stc.ChangeScene("MainMenu", false);
    }
}
