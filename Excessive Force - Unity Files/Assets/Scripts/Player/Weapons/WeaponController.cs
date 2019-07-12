using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform projectileSpawnPoint;

    public GameObject projectilePrefab;

    public float projectileForce = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Fire
            GameObject newProjectile = Instantiate(projectilePrefab);

            newProjectile.transform.position = projectileSpawnPoint.transform.position;
            newProjectile.transform.rotation = projectileSpawnPoint  .transform.rotation;
            newProjectile.GetComponent<Rigidbody>().AddForce(projectileSpawnPoint.transform.forward * projectileForce);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload
        }
    }
}
