using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Класс управления здоровьем игрока, наследуется от Singleton для глобального доступа
public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }                // Флаг смерти игрока

    [SerializeField] private int maxHealth = 3;             // Максимальное здоровье
    [SerializeField] private float knockBackThrustAmount = 10f;    // Сила отбрасывания при получении урона
    [SerializeField] private float damageRecoveryTime = 1f;        // Время восстановления после получения урона

    private Slider healthSlider;                            // Слайдер отображения здоровья
    private int currentHealth;                              // Текущее здоровье
    private bool canTakeDamage = true;                      // Флаг возможности получения урона
    private KnockBack knockback;                            // Компонент отбрасывания
    private Flash flash;                                    // Компонент эффекта вспышки

    const string HEALTH_SLIDER_TEXT = "Health Slider";      // Имя объекта слайдера здоровья
    const string TOWN_TEXT = "Scene1";                      // Имя сцены для перезагрузки
    readonly int DEATH_HASH = Animator.StringToHash("Death"); // Хеш анимации смерти

    // Инициализация компонентов при создании объекта
    protected override void Awake() {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    // Начальная настройка при старте
    private void Start() {
        isDead = false;
        currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    // Обработка столкновений с врагами
    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy) {
            TakeDamage(1, other.transform);
        }
    }

    // Восстановление здоровья игрока
    public void HealPlayer() {
        if (currentHealth < maxHealth) {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    // Получение урона игроком
    public void TakeDamage(int damageAmount, Transform hitTransform) {
        if (!canTakeDamage || SceneManager.GetActiveScene().name == "Menu") { return; }

        ScreenShakeManager.Instance.ShakeScreen();          // Эффект тряски экрана
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);  // Отбрасывание
        StartCoroutine(flash.FlashRoutine());              // Эффект вспышки
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    // Корутина восстановления после получения урона
    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    // Проверка смерти игрока
    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0 && !isDead) {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);      // Уничтожение активного оружия
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH); // Запуск анимации смерти
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    // Корутина перезагрузки сцены после смерти
    private IEnumerator DeathLoadSceneRoutine() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
    }

    // Обновление слайдера здоровья
    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT)?.GetComponent<Slider>();
            if (healthSlider == null) {
                Debug.LogError($"Не удалось найти слайдер здоровья с именем {HEALTH_SLIDER_TEXT}. Убедитесь, что он существует в сцене и имеет компонент Slider.");
                return;
            }
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
