using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class PickUpController : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider coll;
    private GameObject player, ItemHand;
    private Camera cam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ItemHand = player.transform.Find("ItemHand").gameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //Setup
        rb.isKinematic = false;
        coll.isTrigger = false;
    }

    private void Update()
    {
        Vector3 distanceToPlayer = player.transform.position - transform.position;
        GameObject infoUI = transform.Find("infoUI").gameObject;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && infoUI.active == true)
        {
            if ((transform.tag == "Item" && player.GetComponent<PlayerAction>().Inventory[4] == null) || (transform.tag == "MeleeWeapon" && player.GetComponent<PlayerAction>().Inventory[0] == null) )
            {
                PickUp();
            }
        }
    }

    private void PickUp()
    {
        rb.isKinematic = true;

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(ItemHand.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        coll.isTrigger = true;

        player.GetComponent<PlayerAction>().PickItem(transform.gameObject);

        //Enable script
    }

    public void Drop()
    {
        transform.SetParent(GameObject.Find("Items").transform);

        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mouseScreenPosition);


        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 mouseWorldPosition = ray.GetPoint(rayDistance);
            Vector3 dropDirection = (mouseWorldPosition - transform.position).normalized;

            rb.AddForce(dropDirection * dropForwardForce, ForceMode.Impulse);
            rb.AddForce(new Vector3(0,dropUpwardForce, 0), ForceMode.Impulse);
        }

        player.GetComponent<PlayerAction>().DropItem(transform.gameObject);
    }
}
