using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectile : ProjectileController
{
    public override void OnProjectileHit(Collision collision)
    {
        Rigidbody theRB = this.GetComponent<Rigidbody>();
        theRB.velocity = Vector3.zero;
        theRB.useGravity = false;
        theRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        theRB.isKinematic = true;
    }
}
