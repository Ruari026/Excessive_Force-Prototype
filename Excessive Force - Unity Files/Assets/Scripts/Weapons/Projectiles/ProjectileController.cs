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
        string s = collision.gameObject.tag;
        if (s != "Projectile".ToString() && s != source.ToString())
        {
            OnProjectileHit(collision);

            if (collision.gameObject.GetComponent<EntityDamageController>())
            {
                EntityDamageController dc = collision.gameObject.GetComponent<EntityDamageController>();

                // Damaging Hit Entitys
                dc.DamageEntity(this.damage, collision);
            }
        }
    }

    public virtual void OnProjectileHit(Collision collision)
    {
        Destroy(this.gameObject);
    }
}