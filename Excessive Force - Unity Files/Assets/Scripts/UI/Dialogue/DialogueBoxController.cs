using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour
{
    private static DialogueBoxController _instance;
    public static DialogueBoxController Instance
    {
        get
        {
            return _instance;
        }
    }

    // Dialogue Parameters
    public PlayerController interactingPlayer;
    public NpcController interactingNpc;
    private DialogueTree dialogueTree;
    private DialogueOption currentOption;

    // UI Elements
    [SerializeField] List<Text> npcTexts;
    [SerializeField] List<Text> playerOptionTexts;
    public RawImage camImage;

    // Animation Parameters
    private Animator theAnimController;

    [Header("Background Animation")]
    [SerializeField] private Image[] uiBackgroundSprites;
    [SerializeField] private Sprite[] possibleBackgroundSprites;
    [SerializeField] private Vector2 timeBetweenSpriteSwaps;
    private Coroutine backgroundAnim = null;

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

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
    ====================================================================================================
    Dialogue Box Access
    ====================================================================================================
    */
    public void StartDialogue(PlayerController thePlayer, NpcController theNPC)
    {
        interactingPlayer = thePlayer;
        interactingNpc = theNPC;

        dialogueTree = theNPC.npcDialogue.dialogueTree;
        DialogueChange(dialogueTree.dialogueOptions[0]);

        ShowDialogueBox();
    }

    public void EndDialogue()
    {
        HideDialogueBox();
        StartCoroutine(DelayEnd());
    }

    private IEnumerator DelayEnd()
    {
        CameraController theCamera = GameObject.FindObjectOfType<CameraController>();

        CameraMenuState theCameraState = theCamera.cameraMenu;
        theCameraState.targetRotation = Quaternion.Euler(new Vector3(theCamera.cameraGameplay.pitch, theCamera.cameraGameplay.yaw, 0));
        theCameraState.cameraOffset = new Vector3(0.5f, 1.1f, -4.0f);

        yield return new WaitForSeconds(2);
        //yield return new WaitUntil(() => theCamera.transform.rotation == interactingPlayer.transform.rotation);

        // Returning control to the player
        if (interactingPlayer != null)
        {
            interactingPlayer.ChangeState(PlayerStates.STATE_IDLE);
        }
        theCamera.ChangeState(CameraStates.STATE_GAMEPLAY);
        interactingPlayer = null;

        // Progressing NPC State
        if (interactingNpc != null)
        {
            interactingNpc.ChangeState(NpcStates.STATE_IDLE);
            interactingNpc.GetComponent<InteractableObject>().SetCanInteract(true);
            interactingNpc = null;
        }
    }

    private void ShowDialogueBox()
    {
        if (currentOption.type == DialogueType.DIALOGUE_NORMAL)
        {
            theAnimController.SetTrigger("Default");
        }
        else
        {
            theAnimController.SetTrigger("Options");
        }
        theAnimController.SetTrigger("Open");

        backgroundAnim = StartCoroutine(SetBackgroundSprite());
    }

    private void HideDialogueBox()
    {
        theAnimController.SetTrigger("Close");

        StopCoroutine(backgroundAnim);
    }


    /*
    ====================================================================================================
    Dialogue Progressing
    ====================================================================================================
    */
    public void ProgressDialogue(int option = 0)
    {
        if (currentOption.type == DialogueType.DIALOGUE_END)
        {
            EndDialogue();
        }
        else
        {
            DialogueOption nextOption = dialogueTree.dialogueOptions[currentOption.dialogueConnections[option]];

            if (currentOption.type == nextOption.type)
            {
                if (currentOption.type == DialogueType.DIALOGUE_NORMAL)
                {
                    theAnimController.SetTrigger("Default");
                }
                else
                {
                    theAnimController.SetTrigger("Options");
                }

                theAnimController.SetTrigger("StaySame");
            }
            else if (currentOption.type == DialogueType.DIALOGUE_NORMAL)
            {
                theAnimController.SetTrigger("ChangeOptions");
            }
            else if (currentOption.type == DialogueType.DIALOGUE_OPTIONS)
            {
                theAnimController.SetTrigger("ChangeDefault");
            }

            StartCoroutine(DelayDialogueChange(nextOption));
        }
    }

    private void DialogueChange(DialogueOption newDialogue)
    {
        foreach (Text t in npcTexts)
        {
            t.text = newDialogue.npcText;
        }

        if (newDialogue.type == DialogueType.DIALOGUE_OPTIONS)
        {
            for (int i = 0; i < playerOptionTexts.Count; i++)
            {
                playerOptionTexts[i].text = newDialogue.playerOptions[i];
            }
        }

        currentOption = newDialogue;
    }

    private IEnumerator DelayDialogueChange(DialogueOption newDialogue)
    {
        yield return new WaitForSeconds(1);

        DialogueChange(newDialogue);
    }


    /*
    ====================================================================================================
    Dialogue Animation
    ====================================================================================================
    */
    private void AnimateBackground()
    {

    }

    private IEnumerator SetBackgroundSprite()
    {
        float t = Random.Range(timeBetweenSpriteSwaps.x, timeBetweenSpriteSwaps.y);
        yield return new WaitForSeconds(t);

        // Setting next sprite
        int i = Random.Range(0, possibleBackgroundSprites.Length);
        foreach (Image image in uiBackgroundSprites)
        {
            image.sprite = possibleBackgroundSprites[i];
        }

        // Looping Anim
        backgroundAnim = StartCoroutine(SetBackgroundSprite());
    }
}
