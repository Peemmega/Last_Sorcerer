using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

//[RequireComponent(typeof(CharacterController))]

public class EnemyMovement : MonoBehaviour
{

    EnemyStats enemy;
    Transform player;
    Transform playerbase;
    public Transform target;
    public float atkRange;
    public float findTartgetRange;

    private string _class;
    public GameObject mobImage;

    Vector3 knockbackVelocity;
    float knockbackDuration;
    private Vector3 scale = Vector3.one;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        playerbase = GameObject.FindGameObjectWithTag("Base").transform;
        atkRange = enemy.currentAtkRange;
        findTartgetRange = enemy.currentFindTargetRange;
        _class = enemy.currentClass;
    }

    GameObject FindTarget()
    {
        GameObject bestTarget = null;
        float bestDistance = 999;

        for (int i = 0; i < GameObject.Find("LivingThing").transform.GetChildCount(); i++)
        {
            GameObject targeting = GameObject.Find("LivingThing").transform.GetChild(i).gameObject;
            float Distance = Vector3.Distance(transform.position, targeting.transform.position);
            if ((Distance <= findTartgetRange && bestDistance > Distance && targeting.tag != "Base"))
            {
                if (targeting.tag == "Player")
                {
                    return targeting;
                }
                bestDistance = Distance;
                bestTarget = targeting;
            }
        }
       
        return bestTarget;
    }

    void Update()
    {
        GameObject targeting = FindTarget();

        if (targeting != null || (_class == "Assasin"))
        {
            if (targeting == null)
            {
                target = player.transform;
            } else
            {
                target = targeting.transform;
            }
            
        } else 
        { 
            target = playerbase;
        }

        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            
            if (!enemy.onCharging)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                float basedistance = Vector3.Distance(transform.position, playerbase.position);

                if (((target != playerbase) && (distance > atkRange)) || ((target == playerbase) && (basedistance > 2.2 && basedistance > atkRange)))
                {

                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
                }
            }
            
            if (enemy.DashSpeed > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, enemy.DashSpeed * Time.deltaTime);
            }
        }


        if (target.transform.position.x > transform.position.x)
        {
            mobImage.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        }
        else
        {
            mobImage.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }

    public void Knockback(Vector3 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackVelocity.y = 0;
        knockbackDuration = duration;
    }

    public Transform GetPlayer()
    {
        return player;
    }
    public Transform GetPlayerBase()
    {
        return playerbase;
    }

}