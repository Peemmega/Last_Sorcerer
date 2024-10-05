using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemoveWhenRaid : MonoBehaviour
{
    [Header("SafeZone")]    
    public GameDayLoopManager gameDayLoopManager;

    private void Start()
    {
        gameDayLoopManager = FindObjectOfType<GameDayLoopManager>();
    }
    void Update()
    {
        if (gameDayLoopManager.onRaid)
        {
            Destroy(transform.gameObject);
        }
    }
}
