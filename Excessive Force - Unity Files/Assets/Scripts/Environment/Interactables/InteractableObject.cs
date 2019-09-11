using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private GameObject thePlayer;
    private PlayerController thePC;

    [Header("Player Interaction")]
    public bool canInteract = true;
    public float interactRange = 2;

    [Header("UI")]
    public GameObject interactionIcon;
    
    // Start is called before the first frame update
    private void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        thePC = thePlayer.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Player Is Within Interaction Distance
        if (Vector3.Distance(this.transform.position, thePlayer.transform.position) < interactRange && canInteract)
        {
            // Showing Interaction Icon
            interactionIcon.SetActive(true);

            // Rotating Interacion Icon To Face Player Camera
            Vector3 directionToCamera = interactionIcon.transform.position - Camera.main.transform.position;
            interactionIcon.transform.rotation = Quaternion.LookRotation(directionToCamera);

            // Player Is Interacting
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartInteract();
            }
        }
        else
        {
            interactionIcon.SetActive(false);
        }
    }

    public void StartInteract()
    {
        thePC.ChangeState(thePC.playerDisabled);
        CameraController.disabled = true;

        Interaction();
    }

    public virtual void Interaction()
    {
        Debug.Log("Player Interacted With Object");
        EndInteract();
    }

    public void EndInteract()
    {
        thePC.ChangeState(thePC.playerIdle);
        CameraController.disabled = false;
    }
}
