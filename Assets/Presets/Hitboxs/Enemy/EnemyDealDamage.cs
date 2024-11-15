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
        Debug.Log(hit.transform.name);

        if (hit.GetComponent<PlayerStats>())
        {
            //Debug.Log("A");
            PlayerStats player = hit.GetComponent<PlayerStats>();
            player.TakeDamage(Damage);
            enemyStats.DashSpeed = 0;
        } else if (hit.GetComponent<BaseSystem>())
        {
            //Debug.Log("B");
            BaseSystem playerBase = hit.GetComponent<BaseSystem>();
            playerBase.TakeDamage(Damage);
        }
        else if (hit.GetComponent<ShinigamiAI>())
        {
            //Debug.Log("C");
            ShinigamiAI shinigamiAI = hit.GetComponent<ShinigamiAI>();
            shinigamiAI.TakeDamage(Damage);
            enemyStats.DashSpeed = 0;
        }
    }

}
