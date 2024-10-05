using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    // Base Stats for Enemies
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    
    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
    
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }
    
    [SerializeField]
    float attackCD;
    public float AttackCD { get => attackCD; private set => attackCD = value; }


    [SerializeField]
    float atkRange;
    public float AtkRange { get => atkRange; private set => atkRange = value; }

    [SerializeField]
    float chargeDuration;
    public float ChargeDuration { get => chargeDuration; private set => chargeDuration = value; }
    
    [SerializeField]
    float hitboxDuration;
    public float HitboxDuration { get => hitboxDuration; private set => hitboxDuration = value; }

    [SerializeField]
    float findTargetRange;
    public float FindTargetRange { get => findTargetRange; private set => findTargetRange = value; }

    [SerializeField]
    string e_Class;
    public string E_Class { get => e_Class; private set => e_Class = value; }
   
    [SerializeField]
    GameObject hitBox;
    public GameObject Hitbox { get => hitBox; private set => hitBox = value; }

}
