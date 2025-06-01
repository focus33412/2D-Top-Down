using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Компонент, отвечающий за подбираемые предметы в игре
public class Pickup : MonoBehaviour
{
    // Типы подбираемых предметов
    private enum PickUpType
    {
        GoldCoin,       // Золотая монета
        StaminaGlobe,   // Глобус выносливости
        HealthGlobe,    // Глобус здоровья
    }

    [SerializeField] private PickUpType pickUpType;          // Тип подбираемого предмета
    [SerializeField] private float pickUpDistance = 5f;      // Дистанция, на которой предмет начинает притягиваться к игроку
    [SerializeField] private float accelartionRate = .2f;    // Скорость ускорения при притягивании
    [SerializeField] private float moveSpeed = 3f;           // Базовая скорость движения
    [SerializeField] private AnimationCurve animCurve;       // Кривая анимации появления
    [SerializeField] private float heightY = 1.5f;           // Максимальная высота при появлении
    [SerializeField] private float popDuration = 1f;         // Длительность анимации появления

    private Vector3 moveDir;                                 // Направление движения
    private Rigidbody2D rb;                                  // Компонент физики

    // Получение компонента Rigidbody2D при инициализации
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Запуск анимации появления при старте
    private void Start() {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    // Проверка расстояния до игрока и обновление направления движения
    private void Update() {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance) {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelartionRate;
        } else {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    // Применение физического движения к предмету
    private void FixedUpdate() {
        rb.linearVelocity = moveDir * moveSpeed * Time.deltaTime;
    }

    // Обработка столкновения с игроком
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    // Корутина анимации появления предмета
    private IEnumerator AnimCurveSpawnRoutine() {
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 endPoint = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }

    // Обработка эффекта подбора предмета в зависимости от его типа
    private void DetectPickupType() {
        if (SceneManager.GetActiveScene().name == "Menu") {
            return;
        }

        switch (pickUpType)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold();
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break;
            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina(); 
                break;
        }
    }
}
