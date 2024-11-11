using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class ShinigamiAI : MonoBehaviour
{
    EnemyStats enemy;

    [Header("Stats")]
    public float hp;
    float maxHP;
    public float findTartgetRange;
    public float CD;
    public string shikigamiName;
    public GameObject attackFX;
    public ParticleSystem dmgFX;

    public RectTransform hpBar;
    public TextMeshProUGUI nameUI;

    private bool onCD = false;
    float timer;

    GameObject FindTarget()
    {
        for (int i = 0; i < GameObject.Find("enemies").transform.GetChildCount(); i++)
        {
            GameObject enemy = GameObject.Find("enemies").transform.GetChild(i).gameObject;
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if ((enemyDistance <= findTartgetRange))
            {
                Vector3 trackRot = (enemy.transform.position - transform.position).normalized;
                Ray ray = new Ray(transform.position, trackRot);
                LayerMask Mark = LayerMask.GetMask("LivingThing", "Prob", "Enemy");

                if (Physics.Raycast(ray, out RaycastHit hit , 100, Mark))
                {
                    Debug.Log("Attacking: " + hit.transform.name);
                    if (hit.transform.gameObject == enemy)
                    {
                        return enemy;
                    }
                }
            }
        }
        return null;
    }

    void Start()
    {
        maxHP = hp;
        nameUI.text = shikigamiName;
    }
    void Update()
    {
        hpBar.localScale = new Vector2(math.clamp(hp / maxHP, 0, 1), 1);

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


            } else if (shikigamiName == "Vermilion Bird") {
                GameObject hitbox = Instantiate(attackFX);

                Debug.Log(target.name);
                hitbox.transform.position = ((target.transform.position - transform.position).normalized * 0.1f) + transform.position;
                hitbox.transform.LookAt(target.transform.position, transform.position);
                var r = hitbox.transform.eulerAngles;
                hitbox.transform.rotation = Quaternion.Euler(0, r.y + 180, 0);
                Destroy(hitbox, 3f);
            }
        }
    }

    public void TakeDamage(float DMG)
    {
        hp -= DMG; if (hp < 0) {  hp = 0; };

        if (hp <= 0) {
            Destroy(gameObject);
        } else {
            if (dmgFX) Destroy(Instantiate(dmgFX, transform.position, Quaternion.identity), 2f);
        }

    }
}
