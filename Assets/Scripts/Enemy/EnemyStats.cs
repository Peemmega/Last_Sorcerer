using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public RectTransform hpBar;
    public TextMeshProUGUI nameUI;
    public AudioClip deadSound;
    AudioSource audioSource;

    [Header("Current stats")]
    public float currentMoveSpeed;
    public float currentHealth;
    public float currentDamage;
    public float currentAtkRange;
    public string currentClass;
    public float currentFindTargetRange;
    public float currentChargeDuration;
    public float currentHitboxDuration;
    public GameObject currentHitbox;

    public float despawnDistance = 20f;
    Vector3 spawnPos;
    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1); // What the color of the damage flash should be.
    public float damageFlashDuration = 0.2f; // How long the flash should last.
    public float deathFadeTime = 0.6f; // How much time it takes for the enemy to fade.
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    [Header("ATK-CD")]
    public GameObject chargeAlretFX;
    public float currentAttackCD;
    float cdTimer;
    bool isAttackCD;
    public bool onCharging;
    public float DashSpeed;

    void Awake()
    {
        spawnPos = transform.position;
        SetBaseStats();
    }

    //Assign the vaiables
    void SetBaseStats()
    {
        movement = GetComponent<EnemyMovement>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = null;
        nameUI.text = transform.name;


        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currentAtkRange = enemyData.AtkRange;
        currentClass = enemyData.E_Class;
        currentFindTargetRange = enemyData.FindTargetRange;
        currentChargeDuration = enemyData.ChargeDuration;
        currentHitboxDuration = enemyData.HitboxDuration;
        currentAttackCD = enemyData.AttackCD;
        currentHitbox = enemyData.Hitbox;

        sr = transform.Find("Image").GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, 255);
        player = FindObjectOfType<PlayerStats>().transform;
        originalColor = sr.color;
        transform.position = spawnPos;
    }

    void OnEnable()
    {
        SetBaseStats();
    }

    void Start()
    {
        SetBaseStats();
    }

    void Update()
    {
        /* if (Vector2.Distance(transform.position, player.position) >= despawnDistance && gameObject.name != "Cursed Tree")
         {
             ReturnEnemy();
         }*/

        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
        }
        //If the invincibility timer has reached 0, set the invincibility flag to false
        else if (isAttackCD)
        {
            isAttackCD = false;
        }

        hpBar.localScale = new Vector2(currentHealth / enemyData.MaxHealth, 1);
        Transform playerbase = movement.GetPlayerBase();
        Transform target = movement.target;

        float distance = Vector3.Distance(transform.position, movement.target.position);
        float basedistance = Vector3.Distance(transform.position, movement.GetPlayerBase().position);

        // ATK
        if ((!isAttackCD))
        {
            if (!(((target == player) && (distance > currentAtkRange)) || ((target == playerbase) && (basedistance > 2.2 && basedistance > currentAtkRange))))
            {
                Attack(target);
            }
        }
    }

    void Attack(Transform target)
    {
        Debug.Log("Attack to " + target.name);
        cdTimer = currentAttackCD;
        isAttackCD = true;
        StartCoroutine(DoDamage(target));
    }
    IEnumerator DoDamage(Transform target)
    {
        onCharging = true;
        if (target == player)
        {
            Destroy(Instantiate(chargeAlretFX, transform.position, Quaternion.identity, transform), 1f);
        }
        yield return new WaitForSeconds(currentChargeDuration);
        onCharging = false;

        /* if (target.gameObject.GetComponent<PlayerStats>())
         {
             GameObject hitbox = Instantiate(currentHitbox);
             SetHitboxPos(hitbox, target);
             Destroy(hitbox, currentHitboxDuration);
         }
         else if (target.gameObject.GetComponent<BaseSystem>())
         {
             GameObject hitbox = Instantiate(currentHitbox);
             SetHitboxPos(hitbox, target);
             Destroy(hitbox, currentHitboxDuration);

             *//*BaseSystem baseSystem = target.gameObject.GetComponent<BaseSystem>();
             baseSystem.TakeDamage(currentDamage);*//*
         }*/
        if (currentHealth > 0)
        {
            GameObject hitbox = Instantiate(currentHitbox);
            SetHitboxPos(hitbox, target);
            Destroy(hitbox, currentHitboxDuration);
        }
    }
    void SetHitboxPos(GameObject hitbox, Transform target)
    {
        hitbox.GetComponent<EnemyDealDamage>().enemyData = enemyData;
        hitbox.GetComponent<EnemyDealDamage>().enemyStats = this;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dropDirection = (mousePos - transform.position).normalized;

        if (currentClass == "Normal" || currentClass == "Tank")
        {
            if (target.name == "PlayerBase")
            {
                target.GetComponent<BaseSystem>().TakeDamage(enemyData.Damage);
                hitbox.transform.position = target.position;
            }
            else
            {
                hitbox.transform.position = ((target.position - transform.position).normalized * currentAtkRange/1.5f) + transform.position;
            }
        } else if (currentClass == "Ranger")
        {
            hitbox.transform.position = ((target.position - transform.position).normalized * 0.1f) + transform.position;
        }
        else if (currentClass == "Assasin")
        {
            hitbox.transform.parent = transform;
            hitbox.transform.localPosition = Vector3.zero;
            StartCoroutine(Dash(25, 0.3f));
        }   

        hitbox.transform.LookAt(target.position, transform.position);
        var r = hitbox.transform.eulerAngles;
        hitbox.transform.rotation = Quaternion.Euler(0, r.y, 0);
    }

    public void TakeDamage(float dmg, Vector3 sourcePosition, float knockbackForce = 3f, float knockbackDuration = 0.1f)
    {
        if (currentHealth <= 0)
        {
            return;
        }
       
        DashSpeed = 0;
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        // Apply knockback if it is not zero.
        if (knockbackForce > 0)
        {
            // Gets the direction of knockback.
            Vector3 dir = (Vector3)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

        // Kills the enemy if the health drops below zero.
        if (currentHealth <= 0)
        {
            Kill();
        }
    }
    IEnumerator Dash(float spd,float duration)
    {
        DashSpeed = spd;
        yield return new WaitForSeconds(duration);
        DashSpeed = 0;
    }
    IEnumerator DamageFlash()
    {
        sr.material.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.material.color = originalColor;
    }

    public void Kill()
    {
        currentDamage = 0;
        currentMoveSpeed = 0;
        StartCoroutine(KillFade());
    }

    // This is a Coroutine function that fades the enemy away slowly.
    IEnumerator KillFade()
    {
        // Waits for a single frame.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.material.color.a;
        audioSource.clip = deadSound;
        audioSource.Play();
        // This is a loop that fires every frame.
        while (t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            // Set the colour for this frame.
            sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, (1 - t / deathFadeTime) * origAlpha);
        }
        Destroy(gameObject);
    }


    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); 
        }
    }
    private void OnDestroy()
    {
        GameDayLoopManager es = FindObjectOfType<GameDayLoopManager>();
        if (es) es.OnEnemyKilled();
    }

    /* private void OnDestroy()
     {
         EnemySpawner es = FindObjectOfType<EnemySpawner>();
         if (es) es.OnEnemyKilled();
     }

     void ReturnEnemy()
     {
         EnemySpawner es = FindObjectOfType<EnemySpawner>();
         transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
     }*/
}
