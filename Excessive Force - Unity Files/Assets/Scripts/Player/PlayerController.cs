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
    public DodgingState playerDodging;

    // Player Movement Information
    [Header("Ground Movement")]
    public float speed = 15;
    public float groundAccel = 5;
    [Header("Air Movement")]
    public float jumpSpeed = 10;
    public float airAccel = 2.5f;

    // Detecting If Player Is Grounded
    [Header("Player Grounded Info")]
    public float width;
    public float groundedHeight;

    // Controlling Model Hips And Spine Rotations
    public GameObject modelHips;
    public GameObject modelSpine;
    public float hipRotationSpeed = 90;

    //Player Weapon Info
    [Header("Player Weapon")]
    public WeaponController playerWeapon;

    // Other required information for each state
    public Animator animController;
    public GameObject cameraMount;
    public Rigidbody theRB;

    //Temp Variables
    public bool followCameraRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        playerIdle = new IdleState();
        playerMoving = new WalkingState();
        playerJumping = new JumpingState();
        playerFalling = new FallingState();
        playerDodging = new DodgingState();
        
        this.currentState = playerIdle;
    }


    /*
    ====================================================================================================
    Updating States
    ====================================================================================================
    */
    void Update()
    {
        if (followCameraRotation)
        {
            transform.rotation = new Quaternion(0, cameraMount.transform.rotation.y, 0, cameraMount.transform.rotation.w);

            modelSpine.transform.rotation = new Quaternion(0, cameraMount.transform.rotation.y, 0, cameraMount.transform.rotation.w);
            modelSpine.transform.localEulerAngles = new Vector3(cameraMount.transform.eulerAngles.x, modelSpine.transform.localEulerAngles.y, modelSpine.transform.localEulerAngles.z);

        }

        this.currentState.UpdateState(this);

        // Respawn If Player Falls Out Of Bounds
        if (this.transform.position.y < -10)
        {
            this.transform.position = Vector3.zero;
        }

        IsGrounded();
    }

    void FixedUpdate()
    {
        this.currentState.FixedUpdateState(this);
    }
    
    private void OnCollisionStay(Collision collision)
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

        Vector3 force = Vector3.zero;
        force.x = (((input.x * speed) - velocity.x) * acceleration);
        force.z = (((input.y * speed) - velocity.z) * acceleration);
        force = transform.TransformDirection(force);

        theRB.AddForce(force);
    }
    public void MovePlayer(float acceleration, Vector3 direction)
    {
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        Vector3 velocity = transform.InverseTransformDirection(theRB.velocity);

        Vector3 force = Vector3.zero;
        force.x = (((direction.x * speed) - velocity.x) * acceleration);
        force.z = (((direction.y * speed) - velocity.z) * acceleration);
        force = transform.TransformDirection(force);

        theRB.AddForce(force);
    }

    public bool IsGrounded()
    {
        for (int i = 0; i < 5; i++)
        {
            Ray ray = new Ray();

            Vector3 newOrigin = this.transform.position + (Vector3.up * 0.5f);
            switch (i)
            {
                case (1):
                    {
                        newOrigin.x += width / 2;
                        newOrigin.z += width / 2;
                    }
                    break;

                case (2):
                    {
                        newOrigin.x -= width / 2;
                        newOrigin.z += width / 2;
                    }
                    break;

                case (3):
                    {
                        newOrigin.x -= width / 2;
                        newOrigin.z -= width / 2;
                    }
                    break;

                case (4):
                    {
                        newOrigin.x += width / 2;
                        newOrigin.z -= width / 2;
                    }
                    break;
            }
            ray.origin = newOrigin;
            ray.direction = Vector3.down;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.DrawLine(ray.origin, hit.point);
                if (Vector3.Distance(ray.origin, hit.point) <= groundedHeight)
                {
                    return true;
                }
            }
            
        }

        return false;
    }
}
