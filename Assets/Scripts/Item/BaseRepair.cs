using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;

public class BaseRepair : MonoBehaviour
{
    BaseSystem baseSystem;
    PlayerAction playerAction;
    public float HealValue;
    void Start()
    {
        baseSystem = GameObject.FindGameObjectWithTag("Base").GetComponent<BaseSystem>();
        playerAction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAction>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && transform.parent != GameObject.Find("Items").transform)
        {
            Debug.Log("Repair Base");
            baseSystem.RestoreHealth(HealValue);
            playerAction.DropItem(gameObject);
            Destroy(gameObject);
            playerAction.SelectSlot(0);
        }
    }
}
