using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Handling Player Model")]
    public GameObject weaponModel;
    public GameObject modelHand;

    [Header("Handling Aim Point")]
    public GameObject aimMount;
    public GameObject playerSpine;
    public Transform projectileSpawnPoint;
    private Quaternion startRotation;

    [Header("Weapon Stats")]
    public float damage = 1;
    [Range(0,100)]
    public float accuracy = 100;
    public float fireRate = 1;
    public float reloadSpeed = 1;
    public float magazineSize = 1;

    [Header("Weapon Ammo")]
    public GameObject projectilePrefab;
    public float projectileForce = 10;

    private bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = projectileSpawnPoint.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        weaponModel.transform.position = modelHand.transform.position;
        weaponModel.transform.rotation = modelHand.transform.rotation;
        aimMount.transform.rotation = playerSpine.transform.rotation;
    }

    public void Fire()
    {
        if (canFire)
        {
            StartCoroutine(FireGun());
        }
    }

    public void Reload()
    {
        if (canFire)
        {
            StartCoroutine(ReloadGun());
        }
    }

    private IEnumerator FireGun()
    {
        canFire = false;

        while (Input.GetMouseButton(0))
        {
            // Setting the projectile spawn point to face where the crosshairs are facing
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                projectileSpawnPoint.transform.LookAt(hit.point);
            }
            else
            {
                projectileSpawnPoint.transform.localRotation = startRotation;
            }

            // Fire
            GameObject newProjectile = Instantiate(projectilePrefab);

            // Setting Projectile Details
            ProjectileController pc = newProjectile.GetComponent<ProjectileController>();
            pc.SetProjectileStats(this.gameObject.tag, this.damage);

            // Setting Projectile Fire Direction And Speed
            newProjectile.transform.position = projectileSpawnPoint.transform.position;
            newProjectile.transform.rotation = projectileSpawnPoint.transform.rotation;
            newProjectile.GetComponent<Rigidbody>().AddForce(projectileSpawnPoint.transform.forward * projectileForce);

            //Fire Speed
            yield return new WaitForSeconds(1 / fireRate);
        }

        canFire = true;
    }

    private IEnumerator ReloadGun()
    {
        canFire = false;

        //Reload Speed
        yield return new WaitForSeconds(reloadSpeed);

        canFire = true;
    }
}
