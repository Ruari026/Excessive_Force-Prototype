using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectile : ProjectileController
{
    void Update()
    {

    }

    public override void OnProjectileHit(Collision collision)
    {
        // Stopping Projectile At Collision Point
        Rigidbody theRB = this.GetComponent<Rigidbody>();
        theRB.velocity = Vector3.zero;
        theRB.useGravity = false;
        theRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        theRB.isKinematic = true;

        //Preserving Projectile Scale
        Vector3 scale = this.transform.localScale;
        this.transform.SetParent(collision.gameObject.transform, true);
        this.transform.localScale = scale;
    }
}
