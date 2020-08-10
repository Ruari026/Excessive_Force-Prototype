﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{
    private PlayerController thePlayer;

    [Header("Player Interaction")]
    private bool canInteract = true;
    public float interactRange = 4;
    public UnityEvent OnInteraction;

    [Header("UI")]
    public Animator interactionIcon;

    // Update is called once per frame
    private void Update()
    {
        if (thePlayer != null)
        {
            // Player Is Within Interaction Distance
            if (Vector3.Distance(this.transform.position, thePlayer.transform.position) < interactRange && canInteract)
            {
                // Showing Interaction Icon
                interactionIcon.SetBool("CanInteract", true);

                // Rotating Interacion Icon To Face Player Camera
                Vector3 directionToCamera = interactionIcon.gameObject.transform.position - Camera.main.transform.position;
                interactionIcon.gameObject.transform.rotation = Quaternion.LookRotation(directionToCamera);

                // Player Is Interacting
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (OnInteraction != null)
                    {
                        interactionIcon.SetTrigger("Interacted");
                        OnInteraction.Invoke();
                    }
                }
            }
            else
            {
                interactionIcon.SetBool("CanInteract", false);
            }
        }
        else
        {
            thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    public virtual void OnInteract()
    {

    }

    public virtual void OnDisengage()
    {

    }

    public void CanInteract(bool newState)
    {
        canInteract = newState;
    }
}
