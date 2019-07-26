using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public uint maxMagazineSize = 1;
    public uint currentMagazineSize = 1;

    [Header("Weapon Ammo")]
    public GameObject projectilePrefab;
    public float projectileForce = 10;

    private bool canFire = true;

    // Delegate Signatures
    public delegate void EventPlayerGunFire();
    public delegate void EventPlayerGunReload(float reloadTime);

    // Event Instances For EventPlayerGunFire
    public static event EventPlayerGunFire onEventPlayerGunFire;
    // Event Instances For EventPlayerGunReload
    public static event EventPlayerGunReload onEventPlayerGunReload;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = projectileSpawnPoint.transform.localRotation;

        currentMagazineSize = maxMagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        weaponModel.transform.position = modelHand.transform.position;
        weaponModel.transform.rotation = modelHand.transform.rotation;
        aimMount.transform.rotation = playerSpine.transform.rotation;
    }

    // Event Management
    private void OnEnable()
    {
        WeaponController.onEventPlayerGunFire += FireProjectile;
        WeaponController.onEventPlayerGunReload += Reload;
    }
    private void OnDisable()
    {
        WeaponController.onEventPlayerGunFire -= FireProjectile;
        WeaponController.onEventPlayerGunReload -= Reload;
    }

    // Called By Player States That Can Fire During Gameplay
    public void GunInput()
    {
        if (Input.GetMouseButtonDown(0) && canFire)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && canFire)
        {
            WeaponController.onEventPlayerGunReload?.Invoke(this.reloadSpeed);
        }
    }


    /*
    ====================================================================================================
    Gun Firing
    ====================================================================================================
    */
    private void Fire()
    {
        StartCoroutine(FireGun());
    }
    private IEnumerator FireGun()
    {
        canFire = false;

        while (Input.GetMouseButton(0) && currentMagazineSize > 0)
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
            WeaponController.onEventPlayerGunFire?.Invoke();

            // Fire Speed
            yield return new WaitForSeconds(1 / fireRate);
        }

        canFire = true;
    }
    private void FireProjectile()
    {
        GameObject newProjectile = Instantiate(projectilePrefab);

        // Setting Projectile Details
        ProjectileController pc = newProjectile.GetComponent<ProjectileController>();
        pc.SetProjectileStats(this.gameObject.tag, this.damage);

        // Setting Projectile Fire Direction And Speed
        Vector3 newRotation = projectileSpawnPoint.transform.eulerAngles;
        float accuracyAffect = 10 * ((accuracy - 100) / 100);
        newRotation.x += Random.Range(-accuracyAffect, accuracyAffect);
        newRotation.y += Random.Range(-accuracyAffect, accuracyAffect);
        newProjectile.transform.eulerAngles = newRotation;

        newProjectile.transform.position = projectileSpawnPoint.transform.position;
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * projectileForce);

        // Reducing Ammo Count
        currentMagazineSize--;
    }


    /*
    ====================================================================================================
    Gun Reloading
    ====================================================================================================
    */
    private void Reload(float reloadSpeed)
    {
        StartCoroutine(ReloadGun(reloadSpeed));
    }
    private IEnumerator ReloadGun(float reloadSpeed)
    {
        canFire = false;

        //Reload Speed
        yield return new WaitForSeconds(reloadSpeed);
        currentMagazineSize = maxMagazineSize;

        canFire = true;
    }
}