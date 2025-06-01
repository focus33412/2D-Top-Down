using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Класс управления активным оружием игрока, наследуется от Singleton для глобального доступа
public class ActiveWeapon : Singleton<ActiveWeapon>
{
   public MonoBehaviour CurrentActiveWeapon { get; private set; }  // Текущее активное оружие

   private PlayerControls playerControls;                          // Система управления
   private float timeBetweenAttacks;                              // Время между атаками

   private bool attackButtonDown;                                 // Флаг нажатия кнопки атаки
   private bool isAttacking = false;                              // Флаг выполнения атаки

   // Инициализация компонентов при создании объекта
   protected override void Awake() 
   {
    base.Awake();
    playerControls = new PlayerControls();
   }

   // Включение системы управления
   private void OnEnable() 
   {
    playerControls.Enable();
   }

    // Начальная настройка при старте
    private void Start() 
    {
        // Подписка на события нажатия и отпускания кнопки атаки
        playerControls.Player.Attack.started += _ => StartAttacking();
        playerControls.Player.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
   }

    // Обновление каждый кадр
    private void Update() 
    {
        Attack();
    }

    // Установка нового оружия
    public void NewWeapon(MonoBehaviour newWeapon){
        CurrentActiveWeapon = newWeapon;

        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    // Очистка текущего оружия
    public void WeaponNull(){
        CurrentActiveWeapon = null;
    }

    // Запуск перезарядки атаки
    private void AttackCooldown(){
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    // Корутина перезарядки атаки
    private IEnumerator TimeBetweenAttacksRoutine(){
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    // Начало атаки (нажатие кнопки)
    private void StartAttacking(){
        attackButtonDown = true;
    }

    // Окончание атаки (отпускание кнопки)
    private void StopAttacking(){
        attackButtonDown = false;
    }

    // Выполнение атаки
    private void Attack(){
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon){
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
