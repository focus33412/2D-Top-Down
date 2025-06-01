using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс управления активным инвентарем, наследуется от Singleton для глобального доступа
public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;                          // Индекс активного слота

    private PlayerControls playerControls;                       // Система управления

    // Инициализация при создании объекта
    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
    }

    // Начальная настройка при старте
    private void Start() {
        // Подписка на событие переключения слота инвентаря
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    // Включение системы управления
    private void OnEnable() {
        playerControls.Enable();
    }

    // Экипировка начального оружия
    public void EquipStartingWeapon() {
        ToggleActiveHighlight(0);
    }

    // Переключение активного слота
    private void ToggleActiveSlot(int numValue) {
        ToggleActiveHighlight(numValue - 1);
    }

    // Переключение подсветки активного слота
    private void ToggleActiveHighlight(int indexNum) {
        activeSlotIndexNum = indexNum;

        // Отключение подсветки всех слотов
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        // Включение подсветки выбранного слота
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    // Смена активного оружия
    private void ChangeActiveWeapon() {
        // Уничтожение текущего оружия
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null) {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // Получение информации о новом оружии
        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        
        if (inventorySlot == null) {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        
        if (weaponInfo == null) {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        
        if (weaponToSpawn == null) {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        // Создание нового оружия
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        // Установка нового оружия как активного
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
