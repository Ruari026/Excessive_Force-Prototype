using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Base Class For Handling Basic Weapon Stats
/// </summary>
public abstract class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float damage = 1;
    [Range(0, 100)]
    /// <summary>
    /// Affects the angle which the projectile comes out of the spawn point, represented as a percentage
    /// </summary>
    public float accuracy = 100;
    public float fireRate = 1;
    public float reloadSpeed = 1;
    public uint maxMagazineSize = 1;
    public uint currentMagazineSize = 1;

    [Header("Weapon Ammo")]
    public GameObject projectilePrefab;
    public float projectileForce = 10;

    [Header("Fire Point")]
    public Transform projectileSpawnPoint;

    public bool CanFire { get; set; } = true;

    public abstract void GunInput();

    /// <summary>
    /// Fires A Projectile From The Spawn Point And Decreases The Current Ammo Count By 1
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// Sets The Weapon's Ammo Count Back To Max And Prevents Firing For The Reload Speed
    /// </summary>
    /// <param name="reloadTime">The Time That The Gun Will Be Disabled For</param>
    public abstract void Reload(float reloadTime);
}