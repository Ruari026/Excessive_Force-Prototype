using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    [Header("Handling Player Model")]
    public GameObject weaponModel;
    public GameObject modelHand;

    [Header("Handling Aim Point")]
    public GameObject aimMount;
    public GameObject playerSpine;
    private Quaternion startRotation;

    // Delegate Signatures
    public delegate void EventPlayerGunFire();

    public delegate void EventPlayerGunReload(float reloadTime);

    // Event Instances For EventPlayerGunFire
    public static event EventPlayerGunFire OnEventPlayerGunFire;

    // Event Instances For EventPlayerGunReload
    public static event EventPlayerGunReload OnEventPlayerGunReload;

    // Start is called before the first frame update
    void Start()
    {
        this.startRotation = this.projectileSpawnPoint.transform.localRotation;

        this.currentMagazineSize = this.maxMagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        this.weaponModel.transform.position = this.modelHand.transform.position;
        this.weaponModel.transform.rotation = this.modelHand.transform.rotation;

        this.aimMount.transform.rotation = this.playerSpine.transform.rotation;
    }

    // Event Management
    private void OnEnable()
    {
        PlayerWeaponController.OnEventPlayerGunFire += this.FireProjectile;
        PlayerWeaponController.OnEventPlayerGunReload += this.Reload;
    }

    private void OnDisable()
    {
        PlayerWeaponController.OnEventPlayerGunFire -= this.FireProjectile;
        PlayerWeaponController.OnEventPlayerGunReload -= this.Reload;
    }

    /// <summary>
    /// Handles How The Entity Controls The Weapon
    /// </summary>
    public override void GunInput()
    {
        if (Input.GetMouseButtonDown(0) && this.CanFire)
        {
            this.Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && this.CanFire)
        {
            PlayerWeaponController.OnEventPlayerGunReload?.Invoke(this.reloadSpeed);
        }
    }


    /*
    ====================================================================================================
    Gun Firing
    ====================================================================================================
    */
    /// <summary>
    /// Handles How The Weapon Fires Projectiles.
    /// </summary>
    public override void Fire()
    {
        this.StartCoroutine(this.FireGun());
    }

    private IEnumerator FireGun()
    {
        this.CanFire = false;

        while (Input.GetMouseButton(0) && this.currentMagazineSize > 0)
        {
            // Setting the projectile spawn point to face where the crosshairs are facing
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                this.projectileSpawnPoint.transform.LookAt(hit.point);
            }
            else
            {
                this.projectileSpawnPoint.transform.localRotation = this.startRotation;
            }

            // Fire
            PlayerWeaponController.OnEventPlayerGunFire?.Invoke();

            // Fire Speed
            yield return new WaitForSeconds(1 / this.fireRate);
        }

        this.CanFire = true;
    }

    private void FireProjectile()
    {
        GameObject newProjectile = Instantiate(this.projectilePrefab);

        // Setting Projectile Details
        ProjectileController pc = newProjectile.GetComponent<ProjectileController>();
        pc.SetProjectileStats("Player", this.damage);

        // Setting Projectile Fire Direction And Speed
        Vector3 newRotation = this.projectileSpawnPoint.transform.eulerAngles;
        float accuracyAffect = 10 * ((this.accuracy - 100) / 100);
        newRotation.x += Random.Range(-accuracyAffect, accuracyAffect);
        newRotation.y += Random.Range(-accuracyAffect, accuracyAffect);
        newProjectile.transform.eulerAngles = newRotation;

        newProjectile.transform.position = this.projectileSpawnPoint.transform.position;
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * this.projectileForce);

        // Reducing Ammo Count
        this.currentMagazineSize--;
    }


    /*
    ====================================================================================================
    Gun Reloading
    ====================================================================================================
    */
    public override void Reload(float reloadSpeed)
    {
        this.StartCoroutine(this.ReloadGun(reloadSpeed));
    }

    private IEnumerator ReloadGun(float reloadSpeed)
    {
        this.CanFire = false;

        // Reload Speed
        yield return new WaitForSeconds(reloadSpeed);
        this.currentMagazineSize = this.maxMagazineSize;

        this.CanFire = true;
    }
}
