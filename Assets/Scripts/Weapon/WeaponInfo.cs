using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс для хранения информации об оружии, создаваемый как ScriptableObject
[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;           // Префаб оружия
    public float weaponCooldown;              // Время перезарядки
    public int weaponDamage;                  // Урон оружия
    public float weaponRange;                 // Дальность атаки
}
