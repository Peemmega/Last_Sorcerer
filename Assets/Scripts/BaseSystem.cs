using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    public float CurrentHealth = 100;
    private float maxhp = 100;
    public RectTransform hpBar;
    public TextMeshProUGUI hpPercent;
    public ParticleSystem damageEffect;

    void Start()
    {
        maxhp = CurrentHealth;
        UpdateHealthBar();
    }

   /* void Update()
    {
        
    }*/

    public void TakeDamage(float dmg)
    {
        //If the player is not currently invincible, reduce health and start invincibility
        CurrentHealth -= dmg;
        // If there is a damage effect assigned, play it.
        if (damageEffect) Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 5f);

        if (CurrentHealth <= 0)
        {
            /*Kill();*/
            UpdateHealthBar();
            Debug.Log("You Lose");
            Time.timeScale = 0;
        } else
        {
            UpdateHealthBar();
        }

    }
    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum health
        if (CurrentHealth < maxhp)
        {
            CurrentHealth += amount;

            // Make sure the player's health doesn't exceed their maximum health
            if (CurrentHealth > maxhp)
            {
                CurrentHealth = maxhp;
            }
        }
        UpdateHealthBar();
    }
    void UpdateHealthBar()
    {
        //Update the health bar
        hpBar.localScale = new Vector2(CurrentHealth / maxhp, 1);
        hpPercent.text = CurrentHealth + "%";
    }
}
