using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    //Current stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    public float currentSTA;
    float maxSTA;
    float currentMight;
    float currentDef;
    float timescale = 1;


    public ParticleSystem damageEffect;
    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            //Check if the value has changed
            if (currentHealth != value)
            {
                currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            //Check if the value has changed
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //Check if the value has changed
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentDef
    {
        get { return currentDef; }
        set
        {
            //Check if the value has changed
            if (currentDef != value)
            {
                currentDef = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentDefDisplay.text = "Defense: " + currentDef;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            //Check if the value has changed
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }
    #endregion


    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;


    [Header("UI")]
    public GameObject UI;
    TextMeshProUGUI healthText;
    TextMeshProUGUI staminaText;
    TextMeshProUGUI healthMaxText;
    TextMeshProUGUI staminaMaxText;

    [Header("SafeZone")]
    public TextMeshProUGUI locateText;
    public GameDayLoopManager gameDayLoopManager;
    public GameObject fog;
    float fogDamageTimer;

    void Awake()
    {
       
        //Assign the variables
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentDef = characterData.Def;
        maxSTA = characterData.Stamina; ;
        currentSTA = maxSTA;
        //Spawn the starting weapon
        SpawnCharacter(characterData.characterPrefab);
    }

    void Start()
    {
        healthText = UI.transform.Find("HP").Find("currentHP").GetComponent<TextMeshProUGUI>();
        healthMaxText = UI.transform.Find("HP").Find("maxHP").GetComponent<TextMeshProUGUI>();
        staminaText = UI.transform.Find("STA").Find("currentSTA").GetComponent<TextMeshProUGUI>();
        staminaMaxText = UI.transform.Find("STA").Find("maxSTA").GetComponent<TextMeshProUGUI>();

        healthMaxText.text = CurrentHealth.ToString();
        staminaMaxText.text = maxSTA.ToString();

        UpdateHealthBar();
        UpdateStaBar();
    }

   
    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        if (currentSTA < maxSTA && !(transform.GetComponent<PlayerMovement>().GetSprint()))
        {
            timescale += Time.deltaTime * 2;
            currentSTA += Time.deltaTime * 2 * timescale;
            if (currentSTA > maxSTA) { currentSTA = maxSTA; };
        } else
        {
            timescale = 1; 
        }

        if (fogDamageTimer > 0)
        {
            fogDamageTimer -= Time.deltaTime;
        }
       
        fog.SetActive(gameDayLoopManager.onRaid && locateText.text != "Safe zone");

        if (gameDayLoopManager.onRaid && locateText.text != "Safe zone")
        {
            if (fogDamageTimer <= 0)
            {
                TakeDamage(Time.deltaTime);
                fogDamageTimer = 0.3f;
            }
        }

        if (!isInvincible) { }
        {
            isInvincible = false;
        }
        UpdateStaBar();
    }
    public void TakeSTA(float val)
    {
        timescale = 1;
        currentSTA -= val;
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            dmg = math.clamp(dmg - CurrentDef, 1, 99999);

            CurrentHealth -= dmg;

            if (damageEffect) Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 2f);

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }

            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        //Update the health bar
        healthText.text = CurrentHealth.ToString();
    }
    void UpdateStaBar()
    {
        //Update the health bar
        staminaText.text = Mathf.Floor(currentSTA).ToString();
    }
    public void Kill()
    {
        transform.gameObject.SetActive(false);
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum health
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;

            // Make sure the player's health doesn't exceed their maximum health
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
        UpdateHealthBar();
    }

/*    void Recover()
    {

        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            // Make sure the player's health doesn't exceed their maximum health
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }

        UpdateHealthBar();
    }*/

    public void SpawnCharacter(GameObject avatar)
    {
        //Spawn Character
        GameObject character = Instantiate(avatar, transform.position, Quaternion.identity);
        character.transform.SetParent(transform);
        transform.GetComponent<PlayerMovement>().playerSprite = character.GetComponent<SpriteRenderer>();
        transform.GetComponent<PlayerMovement>().anim = character.GetComponent<Animator>();
    }

}
