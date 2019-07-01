using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePads : MonoBehaviour
{
    public float bounceForce = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody otherRB = collision.gameObject.GetComponent<Rigidbody>();
            otherRB.AddForce(this.transform.up * bounceForce);
        }
    }
}
