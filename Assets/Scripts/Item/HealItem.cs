using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;

public class HealItem : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerAction playerAction;
    public float HealValue;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerAction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAction>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && transform.parent != GameObject.Find("Items").transform)
        {
            Debug.Log("Heal Player");
            playerStats.RestoreHealth(HealValue);
            playerAction.DropItem(gameObject);
            Destroy(gameObject);
            playerAction.SelectSlot(0);
        }
    }
}
