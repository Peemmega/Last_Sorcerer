using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public WeaponScriptableObject data;
    GameObject hitbox;
    public float cd;

    void Start()
    {
        hitbox = data.weaponPrefab;
        cd = data.cd;
    }

    public GameObject GetHitbox()
    {
        return hitbox;
    }

 
}
