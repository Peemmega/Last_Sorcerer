using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    /// <summary>
    /// Public fields
    /// </summary>
    public float m_Speed = 10f;   // this is the projectile's speed

    /// <summary>
    /// Private fields
    /// </summary>
    private Rigidbody m_Rigidbody;

    /// <summary>
    /// Message that is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Message that is called before the first frame update
    /// </summary>
    void Start()
    {
        m_Rigidbody.AddForce(m_Rigidbody.transform.forward * -m_Speed);
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!(hit.tag == "Player" || hit.tag == "Barrier" || hit.tag == "Item" || hit.tag == "Shinigami" || hit.tag == "MeleeWeapon"))
        {
            Destroy(gameObject);
        }
    }
}
