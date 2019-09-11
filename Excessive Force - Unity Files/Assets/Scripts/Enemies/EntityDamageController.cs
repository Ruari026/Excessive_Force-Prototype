using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDamageController : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    public delegate void EventEntityDeath(EntityDamageController entity);
    public static EventEntityDeath onEventEntityDeath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEntity(float damage)
    {
        currentHealth -= damage;

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
