using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public ProjectileType pType = ProjectileType.BRITTLE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Projectile")
        {
            switch (pType)
            {
                case (ProjectileType.BRITTLE):
                    {
                        Destroy(this.gameObject);
                    }
                    break;

                case (ProjectileType.PIERCING):
                    {
                        Rigidbody theRB = this.GetComponent<Rigidbody>();
                        theRB.velocity = Vector3.zero;
                        theRB.isKinematic = true;
                        theRB.useGravity = false;
                        theRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                        this.transform.position = collision.GetContact(0).point;
                    }
                    break;
            }
        }
    }
}

public enum ProjectileType
{
    BRITTLE,
    PIERCING,
    BOUNCY
};
