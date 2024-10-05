using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    void Update()
    {
        transform.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
    }
}
