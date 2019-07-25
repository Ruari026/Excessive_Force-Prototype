using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private GameObject thePlayer;
    private PlayerController thePC;

    public float interactRange = 2;
    private bool canInteract = true;

    public List<GameObject> enableItems;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        thePC = thePlayer.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Vector3.Distance(this.transform.position, thePlayer.transform.position) < interactRange && canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartInteract();
            }
        }
    }

    public void StartInteract()
    {
        thePC.ChangeState(thePC.playerDisabled);

        foreach (GameObject g in enableItems)
        {
            g.SetActive(true);
        }

        CameraController.disabled = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EndInteract()
    {
        thePC.ChangeState(thePC.playerIdle);

        foreach (GameObject g in enableItems)
        {
            g.SetActive(false);
        }

        CameraController.disabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
