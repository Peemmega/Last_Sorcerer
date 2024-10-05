using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAnimation : MonoBehaviour
{
    Animator am;
    GameObject player;
    PlayerMovement pm;
    SpriteRenderer sr;
    private Rigidbody rb;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().gameObject;

        sr = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();
        pm = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            am.SetBool("Move", true);
        }
        else
        {
            am.SetBool("Move", false);
        }
    }
}
