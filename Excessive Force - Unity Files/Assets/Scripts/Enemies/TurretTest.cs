using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class For Controlling All Turret Type Enemies.
/// </summary>
public class TurretTest : MonoBehaviour
{
    private enum TurretState { SEARCHING, ATTACKING }
    private TurretState currentState = TurretState.SEARCHING;

    public GameObject attackTarget;

    public float rotationSpeed = 5;
    Vector3 d = Vector3.up;

    public float attackRange = 10;
    
    public GameObject xRotationPoint;
    public GameObject yRotationPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TurretState.SEARCHING:
                {
                    // State Exit Condition
                    if (Vector3.Distance(this.transform.position, attackTarget.transform.position) <= attackRange)
                    {
                        currentState = TurretState.ATTACKING;
                    }

                    // State Behaviour
                    TurretSearching();
                }
                break;

            case TurretState.ATTACKING:
                { 
                    // State Exit Condition
                    if (Vector3.Distance(this.transform.position, attackTarget.transform.position) > attackRange)
                    {
                        currentState = TurretState.SEARCHING;
                    }

                    // State Behaviour
                    TurretAttacking();
                }
                break;
        }
    }


    /*
    ====================================================================================================
    Handling Searching State
    ====================================================================================================
    */
    private void TurretSearching()
    {
       
    }


    /*
    ====================================================================================================
    Handling Attacking State
    ====================================================================================================
    */
    private void TurretAttacking()
    {
        // Turret Should Be Looking At The Player's Torso
        Vector3 lookTarget = attackTarget.transform.position;
        lookTarget.y += 1;

        // Affecting Rotation On The Y Axis
        Vector3 lookDirectionY = lookTarget - yRotationPoint.transform.position;
        lookDirectionY.y = 0;
        lookDirectionY.Normalize();
        // Rotating To The New Directon
        Quaternion newRotationY = Quaternion.LookRotation(lookDirectionY, Vector3.up);
        yRotationPoint.transform.rotation = Quaternion.Lerp(yRotationPoint.transform.rotation, newRotationY, Time.deltaTime * rotationSpeed);

        // Affecting Rotation On The X Axis
        float distanceToTarget = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));
        Vector2 lookDirectionX = new Vector2(distanceToTarget, (attackTarget.transform.position.y - xRotationPoint.transform.position.y + 1));
        float angle = lookDirectionX.y / lookDirectionX.x;
        angle = Mathf.Atan(angle);
        angle = angle * -180 / Mathf.PI;
        xRotationPoint.transform.localEulerAngles = new Vector3(angle, 0, 0);
    }
}
