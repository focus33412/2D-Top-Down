using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс управления луком, реализующий интерфейс IWeapon
public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;              // Информация об оружии
    [SerializeField] private GameObject arrowPrefab;             // Префаб стрелы
    [SerializeField] private Transform arrowSpawnPoint;          // Точка появления стрелы

    readonly int FIRE_HASH = Animator.StringToHash("Fire");      // Хеш параметра стрельбы для аниматора

    private Animator myAnimator;                                 // Компонент аниматора

    // Инициализация компонентов при создании
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Выполнение атаки
    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
    }

    // Получение информации об оружии
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
