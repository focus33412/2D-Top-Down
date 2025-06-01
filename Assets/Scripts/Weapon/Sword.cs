using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Класс управления мечом, реализующий интерфейс IWeapon
public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;         // Префаб анимации удара
    [SerializeField] private Transform slashAnimSpawnPoint;      // Точка появления анимации
    [SerializeField] private float swordAttackCooldown = 0.5f;   // Время перезарядки атаки
    [SerializeField] private WeaponInfo weaponInfo;              // Информация об оружии

    private Transform weaponCollider;                            // Коллайдер оружия
    private Animator myAnimator;                                 // Компонент аниматора
    private GameObject slashAnim;                                // Объект анимации удара

    // Инициализация компонентов при создании
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Начальная настройка при старте
    private void Start() {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashspawnPoint").transform;
    }

    // Обновление каждый кадр
    private void Update(){
        MouseFollowWithOffset();
    }

    // Получение информации об оружии
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    // Выполнение атаки
    public void Attack()
    {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    // Событие завершения анимации атаки
    public void DoneAttackingAnimEvent(){
        weaponCollider.gameObject.SetActive(false);
    }   

    // Событие поворота анимации удара вверх
    public void SwingUpFlipAnimEvent() {
        if (slashAnim == null) return;
        
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
    }

    // Событие поворота анимации удара вниз
    public void SwingDownFlipAnimEvent() {
        if (slashAnim == null) return;
        
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Поворот оружия за курсором с учетом смещения
    private void MouseFollowWithOffset(){
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        // Поворот оружия и анимации в зависимости от положения курсора
        if(mousePosition.x < playerScreenPoint.x){
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            if (slashAnim != null)
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        } else {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (slashAnim != null)
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}