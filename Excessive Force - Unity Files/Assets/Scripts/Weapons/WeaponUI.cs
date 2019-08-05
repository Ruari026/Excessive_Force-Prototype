using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public WeaponController targetWeapon;

    // Ammo Information
    public Text ammoCountText;

    // Reloading Information
    public Image reloadBar;

    // Start is called before the first frame update
    void Start()
    {
        GetWeaponInformation();
    }

    // Event Management
    private void OnEnable()
    {
        PlayerWeaponController.OnEventPlayerGunFire += GetWeaponInformation;
        PlayerWeaponController.OnEventPlayerGunReload += RunReloadUI;
    }
    private void OnDisable()
    {
        PlayerWeaponController.OnEventPlayerGunFire -= GetWeaponInformation;
        PlayerWeaponController.OnEventPlayerGunReload -= RunReloadUI;
    }


    /*
    ====================================================================================================
    Gun Firing
    ====================================================================================================
    */
    private void GetWeaponInformation()
    {
        ammoCountText.text = "Ammo: " + targetWeapon.currentMagazineSize + "/" + targetWeapon.maxMagazineSize;
    }


    /*
    ====================================================================================================
    Gun Reloading
    ====================================================================================================
    */
    public void RunReloadUI(float reloadTime)
    {
        StartCoroutine(ReloadUI(reloadTime));
    }
    private IEnumerator ReloadUI(float time)
    {
        float t = 0;
        reloadBar.rectTransform.localScale = new Vector3(0, 1, 1);

        while (t < 1)
        {
            t += Time.deltaTime / time;
            reloadBar.rectTransform.localScale = new Vector3(t, 1, 1);

            yield return new WaitForEndOfFrame();
        }

        reloadBar.rectTransform.localScale = new Vector3(0, 1, 1);
        ammoCountText.text = "Ammo: " + targetWeapon.maxMagazineSize + "/" + targetWeapon.maxMagazineSize;
    }
}
