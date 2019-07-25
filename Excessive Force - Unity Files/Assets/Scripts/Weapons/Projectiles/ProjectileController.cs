using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Projectile Source
    public string source = "Player";

    // Projectile Stats
    public float damage = 1;
    public float elementalDamage = 1;
    public float elementalChance = 1;

    public void SetProjectileStats(string source, float damage)
    {
        this.source = source;
        this.damage = damage;
    }
    public void SetProjectileStats(string source, float damage, float elementalDamage, float elementalChance)
    {
        this.source = source;
        this.damage = damage;
        this.elementalDamage = elementalDamage;
        this.elementalChance = elementalChance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Projectile" && collision.gameObject.tag != source)
        {
            if (collision.gameObject.GetComponent<EntityDamageController>())
            {
                EntityDamageController dc = collision.gameObject.GetComponent<EntityDamageController>();

                // Damaging Hit Entitys
                dc.DamageEntity(this.damage);
            }

            OnProjectileHit(collision);
            // ToDo: Use This For Subclassed Projectile Controllers
            /*switch (pType)
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
            }*/
        }
    }

    public virtual void OnProjectileHit(Collision collision)
    {
        Destroy(this.gameObject);
    }
}