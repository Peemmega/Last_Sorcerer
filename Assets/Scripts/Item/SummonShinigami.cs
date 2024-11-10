using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SummonShinigami : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerAction playerAction;
    public GameObject Shinigami;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerAction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAction>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && transform.parent != GameObject.Find("Items").transform)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            LayerMask Mark = LayerMask.GetMask("LivingThing", "Prob");

            if (Physics.Raycast(ray, out hit, 100, Mark) && hit.transform.name == "Shinigami Area")
            {
                Vector3 pos = hit.point - new Vector3(0, hit.point.y - 0.65f, 0) ;
                GameObject shinigami = Instantiate(Shinigami, pos, Quaternion.identity);
                shinigami.transform.parent = GameObject.Find("LivingThing").transform;
                playerAction.DropItem(gameObject);
                Destroy(gameObject);
                playerAction.SelectSlot(0);
            }
          
          
        }
    }
}
