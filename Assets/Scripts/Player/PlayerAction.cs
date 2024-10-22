using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Image = UnityEngine.UI.Image;

public class PlayerAction : MonoBehaviour
{
    private GameObject player, ItemHand;
    private Camera cam;
    public Vector3 MouseDirection;
    public GameObject fistHitbox;
    public GameObject[] Inventory = new GameObject[5];

    public GameObject itemSlotUI;
    public GameObject[] invSlots = new GameObject[5];
    int selectedSlot = 0;
    Sprite FistIcon;


    [Header("Combat")]
    public float cdTimer;
    public int combo;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ItemHand = player.transform.Find("ItemHand").gameObject; 
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        invSlots[0] = itemSlotUI.transform.Find("MeleeWeapon").gameObject;
        invSlots[1] = itemSlotUI.transform.Find("Inventory1").gameObject;
        invSlots[2] = itemSlotUI.transform.Find("Inventory2").gameObject;
        invSlots[3] = itemSlotUI.transform.Find("Inventory3").gameObject;
        invSlots[4] = itemSlotUI.transform.Find("Inventory4").gameObject;

        FistIcon = invSlots[0].transform.Find("Icon").GetComponent<Image>().sprite;
        SelectSlot(0);
    }

  /*  IEnumerator WaitAndPrint(GameObject test)
    {
        yield return new WaitForSeconds(0.3f);
        
    }
*/
    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mouseScreenPosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(rayDistance);
            MouseDirection = (mouseWorldPosition - transform.position).normalized;
        }

        if (Input.GetMouseButtonDown(0) && (selectedSlot == 0 || (Inventory[selectedSlot] != null && Inventory[selectedSlot].GetComponent<MeleeWeapon>())))
        {
            Attack();
        }

        if (Input.GetKeyDown("1"))
        {
            SelectSlot(0);
        }

        if (Input.GetKeyDown("2"))
        {
            SelectSlot(1);
        }

        if (Input.GetKeyDown("3"))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyDown("4"))
        {
            SelectSlot(3);
        }

        if (Input.GetKeyDown("5"))
        {
            SelectSlot(4);
        }

        if (Input.GetKeyDown(KeyCode.X) && (Inventory[selectedSlot] != null))
        {
            DropItem(Inventory[selectedSlot]);
        }
        if (combo == 3)
        {
            combo = 0;
            cdTimer = .6f;
        }

        if (combo > 0 && cdTimer <= -1.5)
        {
            Debug.Log("Reset combo");
            combo = 0;
            cdTimer = .6f;
        }
        if (cdTimer > -1.5f)
        {
            cdTimer -= Time.deltaTime;
        }

        /* if (Input.GetMouseButtonDown(1))
             Debug.Log("Pressed right-click.");*/
    }

    void Attack()
    {
        float stamina = transform.GetComponent<PlayerStats>().currentSTA;
        if (cdTimer > 0 || stamina <= 5)
        {
            return;
        }

        transform.GetComponent<PlayerStats>().TakeSTA(5);

        if (Inventory[0] == null && selectedSlot == 0) {
            GameObject hitbox = Instantiate(fistHitbox);
            cdTimer = .2f;
            SetHitboxPos(hitbox);
            Destroy(hitbox, 0.05f);
        } else
        {
            GameObject hitbox = Instantiate(Inventory[selectedSlot].transform.GetComponent<MeleeWeapon>().GetHitbox());
            cdTimer = Inventory[selectedSlot].transform.GetComponent<MeleeWeapon>().GetCD();
            hitbox.GetComponent<DealDamage>().weaponData = Inventory[selectedSlot].transform.GetComponent<MeleeWeapon>().data;
            SetHitboxPos(hitbox);
            Destroy(hitbox, Inventory[selectedSlot].transform.GetComponent<MeleeWeapon>().GetHitboxLifeTime());
        }

        combo++;
    }

    void SetHitboxPos(GameObject hitbox)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dropDirection = (mousePos - transform.position).normalized;
        hitbox.transform.position = (MouseDirection * 1f) + player.transform.position;
        hitbox.transform.LookAt(transform.position, dropDirection);
        var r = hitbox.transform.eulerAngles;
        hitbox.transform.rotation = Quaternion.Euler(0, r.y, 0);
    }
    void ChangeActive(bool yo)
    {
        if (Inventory[selectedSlot] != null)
        {
            Inventory[selectedSlot].SetActive(yo);
        }
    }

    public void SelectSlot(int slot)
    {
        ChangeActive(false);
        selectedSlot = slot;
        ChangeActive(true);

        for (int i = 0; i < 5; i++)
        {
            GameObject currentSlot = invSlots[i];
            Image itemImage = currentSlot.GetComponent<Image>();

            itemImage.color = new Color32(66, 66, 66, 255);
            if (i == slot)
            {
                itemImage.color = new Color32(255, 255, 255, 255);
            }
        }
    }

    void resetPic() {
        if (selectedSlot != 0)
        {
            invSlots[selectedSlot].transform.Find("Icon").GetComponent<Image>().sprite = null;
            invSlots[selectedSlot].transform.Find("Icon").GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
        else
        {
            invSlots[selectedSlot].transform.Find("Icon").GetComponent<Image>().sprite = FistIcon;
            invSlots[selectedSlot].transform.Find("Icon").GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
    }
    void UpdatePic()
    {
        invSlots[selectedSlot].transform.Find("Icon").GetComponent<Image>().sprite = Inventory[selectedSlot].transform.Find("ItemPic").transform.GetComponent<SpriteRenderer>().sprite;
        invSlots[selectedSlot].transform.Find("Icon").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void PickItem(GameObject item)
    {
        if (item.tag == "Item")
        {
            for (int i = 1;i <= 4; i++)
            {
                if (Inventory[i] == null)
                {
                    //Debug.Log("Collect item: " + item.name);
                    Inventory[i] = item;
                    SelectSlot(i);
                    break;
                }
            }
        }
        else if (item.tag == "MeleeWeapon")
        {
            //Debug.Log("Pick MeleeWeapon: " + item.name);
            if (Inventory[0] == null)
            {
                Inventory[0] = item;
                SelectSlot(0);
            }
        }

        UpdatePic();
    }

    public void DropItem(GameObject item)
    {
        if (Inventory[selectedSlot] != null)
        {
            GameObject selectItem = Inventory[selectedSlot];

            if (item.tag == "Item")
            {
                //Debug.Log("Drop item: " + item.name);
                Inventory[selectedSlot] = null;
                selectItem.transform.GetComponent<PickUpController>().Drop();
                
            }

            else if (item.tag == "MeleeWeapon")
            {
                //Debug.Log("Drop MeleeWeapon: " + item.name);
                Inventory[selectedSlot] = null;
                selectItem.transform.GetComponent<PickUpController>().Drop();

            }

            resetPic();
        }
       
    }

}
