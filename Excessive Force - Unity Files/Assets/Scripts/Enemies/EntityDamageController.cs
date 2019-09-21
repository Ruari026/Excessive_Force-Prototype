using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDamageController : MonoBehaviour
{
    [Header("Controlling Health")]
    public float currentHealth;
    public float maxHealth;

    [Header("Showing Damage")]
    public GameObject damageTextPrefab;

    public delegate void EventEntityDeath(EntityDamageController entity);
    public static EventEntityDeath onEventEntityDeath;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DamageEntity(float damage, Collision collision)
    {
        currentHealth -= damage;

        GameObject g = Instantiate(damageTextPrefab);
        g.transform.position = collision.contacts[0].point;

        DamageTextController dtc = g.GetComponent<DamageTextController>();
        dtc.SetDamageText(damage);

        if (currentHealth <= 0)
        {
            EntityDamageController.onEventEntityDeath?.Invoke(this);
            KillEntity();
        }
    }

    public void KillEntity()
    {

    }
}
