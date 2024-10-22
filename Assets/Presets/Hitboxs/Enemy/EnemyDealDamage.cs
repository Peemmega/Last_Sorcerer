using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public EnemyStats enemyStats;
    float Damage = 0;
   
    void Start()
    {
        Damage = enemyData.Damage;
    }

    private void OnTriggerEnter(Collider hit)
    {
        //Debug.Log(hit.GetComponent<BaseSystem>());
        if (hit.GetComponent<PlayerStats>())
        {
            PlayerStats player = hit.GetComponent<PlayerStats>();
            player.TakeDamage(Damage);
            enemyStats.DashSpeed = 0;
        } else if (hit.GetComponent<BaseSystem>())
        {
            BaseSystem playerBase = hit.GetComponent<BaseSystem>();
            playerBase.TakeDamage(Damage);
        }
    }

}
