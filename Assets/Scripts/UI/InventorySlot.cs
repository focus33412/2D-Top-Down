using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс слота инвентаря для хранения информации об оружии
public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;              // Информация об оружии в слоте

    // Получение информации об оружии
    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }
}
