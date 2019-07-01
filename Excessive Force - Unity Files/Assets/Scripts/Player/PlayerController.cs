using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // The state the player is currently in
    private PlayerState currentState;

    // All possible states for the player to be in
    public IdleState playerIdle;
    public WalkingState playerMoving;
    public JumpingState playerJumping;
    public FallingState playerFalling;

    // Player Movement Information
    [Header("Ground Movement")]
    public float speed = 15;
    public float groundAccel = 5;
    [Header("Air Movement")]
    public float jumpSpeed = 10;
    public float airAccel = 2.5f;

    // Other required information for each state
    public Animator animController;
    public GameObject cameraMount;
    public Rigidbody theRB;
    
    // Start is called before the first frame update
    void Start()
    {
        playerIdle = new IdleState();
        playerMoving = new WalkingState();
        playerJumping = new JumpingState();
        playerFalling = new FallingState();

        this.currentState = playerIdle;
    }


    /*
    ====================================================================================================
    Updating States
    ====================================================================================================
    */
    void Update()
    {
        transform.rotation = new Quaternion(0, cameraMount.transform.rotation.y, 0, cameraMount.transform.rotation.w);
        this.currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        this.currentState.FixedUpdateState(this);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        currentState.CheckCollisionState(this, collision);
    }
    

    /*
    ====================================================================================================
    Handling State Changes
    ====================================================================================================
    */
    public void ChangeState(PlayerState newState)
    {
        Debug.Log("Player Has Entered New State: " + newState);

        this.currentState = newState;
        this.currentState.StartState(this);
    }

    public IEnumerator ChangeStateTemporary(PlayerState tempState, float time)
    {
        PlayerState stateBefore = this.currentState;
        this.currentState = tempState;

        yield return new WaitForSeconds(time);

        this.currentState = stateBefore;
    }
    public IEnumerator ChangeStateTemporary(PlayerState tempState, PlayerState newState, float time)
    {
        this.currentState = tempState;

        yield return new WaitForSeconds(time);

        this.currentState = newState;
    }
    
    
    /*
    ====================================================================================================
    Player Movement
    ====================================================================================================
    */
    public void MovePlayer(float acceleration)
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.magnitude > 1)
        {
            input.Normalize();
        }

        Vector3 velocity = transform.InverseTransformDirection(theRB.velocity);
        Vector3 force = new Vector3
        {
            x = (((input.x * speed) - velocity.x) * acceleration),
            y = 0,
            z = (((input.y * speed) - velocity.z) * acceleration)
        };
        force = transform.TransformDirection(force);
        theRB.AddForce(force);
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, out hit);
        if (Vector3.Distance(transform.position, hit.point) < 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
