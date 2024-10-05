using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "Weapons/WeaponScriptableObject")]
public class WeaponScriptableObject : ScriptableObject
{
    public Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

    public GameObject weaponPrefab;
    public GameObject WeaponPrefab { get => weaponPrefab; private set => weaponPrefab = value;}

    public float damage;
    public float Damage { get => damage; private set => damage = value; }

    public float cd;
    public float CD { get => cd; private set => cd = value; }

}
