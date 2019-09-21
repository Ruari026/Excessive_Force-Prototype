using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextController : MonoBehaviour
{
    public Text[] damageTexts = new Text[2];

    private void Start()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        float r = Random.Range(-60, 60);
        Vector3 direction = Quaternion.Euler(0, r, 0) * (Camera.main.transform.position - this.transform.position).normalized * 2500;
        direction += Vector3.up * 1000;

        rb.AddForce(direction);
    }

    private void Update()
    {
        Vector3 lookDirection = this.transform.position - Camera.main.transform.position;
        this.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    public void SetDamageText(float damageValue)
    {
        string damage = damageValue.ToString();
        if (damageValue >= 1000000000000000000)
        {
            damage = (damageValue / 1000000000000000000).ToString("0.00") + "E";
        }
        else if (damageValue >= 1000000000000000)
        {
            damage = (damageValue / 1000000000000000).ToString("0.00") + "P";
        }
        else if (damageValue >= 1000000000000)
        {
            damage = (damageValue / 1000000000000).ToString("0.00") + "T";
        }
        else if (damageValue >= 1000000000)
        {
            damage = (damageValue / 1000000000).ToString("0.00") + "G";
        }
        else if (damageValue >= 1000000)
        {
            damage = (damageValue / 1000000).ToString("0.00") + "M";
        }
        else if (damageValue >= 1000)
        {
            damage = (damageValue / 1000).ToString("0.00") + "k";
        }

        foreach (Text t in damageTexts)
        {
            t.text = damage;
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
