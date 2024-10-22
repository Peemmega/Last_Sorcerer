using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinigamiAI : MonoBehaviour
{
    EnemyStats enemy;

    [Header("Stats")]
    public float findTartgetRange;
    public float CD;
    public string shikigamiName;
    public GameObject attackFX;

    private bool onCD = false;
    float timer;

    GameObject FindTarget()
    {
        for (int i = 0; i < GameObject.Find("enemies").transform.GetChildCount(); i++)
        {
            GameObject enemy = GameObject.Find("enemies").transform.GetChild(i).gameObject;
            //Debug.Log(enemy.name);
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if ((enemyDistance <= findTartgetRange))
            {
                return enemy;
            }
        }
        return null;
    }

    void Update()
    {
        if (onCD) { timer -= Time.deltaTime; if (timer <= 0) {onCD = false;} else return; }
        
        GameObject target = FindTarget();
        if (target != null)
        {
            onCD = true;
            timer = CD;
            // Attack
            if (shikigamiName == "Azure Dragon") {
                target.GetComponent<EnemyStats>().TakeDamage(10f, target.transform.position);
                Destroy(Instantiate(attackFX, target.transform),2f);
            }
        }
    }
}
