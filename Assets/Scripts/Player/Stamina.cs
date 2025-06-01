using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Класс управления выносливостью игрока, наследуется от Singleton для глобального доступа
public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }         // Текущее количество выносливости

    [SerializeField] private Sprite fullStaminaImage;       // Спрайт полной выносливости
    [SerializeField] private Sprite emptyStaminaImage;      // Спрайт пустой выносливости
    [SerializeField] private int timeBetweenStaminaRefresh = 3;  // Время между восстановлением выносливости

    private Transform staminaContainer;                      // Контейнер для отображения выносливости
    private int startingStamina = 3;                        // Начальное количество выносливости
    private int maxStamina;                                 // Максимальное количество выносливости
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";  // Имя объекта контейнера выносливости

    // Инициализация компонентов при создании объекта
    protected override void Awake() {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    // Начальная настройка при старте
    private void Start() {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    // Использование выносливости
    public void UseStamina() {
        CurrentStamina--;
        UpdateStaminaImages();
    }

    // Восстановление выносливости
    public void RefreshStamina() {
        if (CurrentStamina < maxStamina) {
            CurrentStamina++;
        }
        UpdateStaminaImages();
    }

    // Корутина автоматического восстановления выносливости
    private IEnumerator RefreshStaminaRoutine() {
        while (true) {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    // Обновление отображения выносливости
    private void UpdateStaminaImages() {
        // Обновляем спрайты для каждого индикатора выносливости
        for (int i = 0; i < maxStamina; i++) {
            if (i <= CurrentStamina - 1) {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
            } else {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaImage;
            }
        }

        // Запускаем восстановление, если выносливость не полная
        if (CurrentStamina < maxStamina) {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }
}
