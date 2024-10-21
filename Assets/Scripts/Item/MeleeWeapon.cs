using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public WeaponScriptableObject data;
    GameObject hitbox;
    float cd;
    float lifeTime;
    void Start()
    {
        hitbox = data.weaponPrefab;
        cd = data.cd;
        lifeTime = data.lifeTime;
    }

    public GameObject GetHitbox()
    {
        return hitbox;
    }

    public float GetCD()
    {
        return cd;
    }

    public float GetHitboxLifeTime()
    {
        return lifeTime;
    }
}
