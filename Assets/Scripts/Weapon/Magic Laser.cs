using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс управления магическим лучом
public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;           // Время роста луча

    private bool isGrowing = true;                              // Флаг роста луча
    private float laserRange;                                   // Дальность луча
    private SpriteRenderer spriteRenderer;                      // Компонент отрисовки спрайта
    private CapsuleCollider2D capsuleCollider2D;                // Компонент коллайдера

    // Инициализация компонентов при создании
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Начальная настройка при старте
    private void Start() {
        LaserFaceMouse();
    }

    // Обработка столкновения с неразрушаемым объектом
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Indestructible>() && !other.isTrigger) {
            isGrowing = false;
        }
    }

    // Обновление дальности луча
    public void UpdateLaserRange(float laserRange) {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    // Корутина увеличения длины луча
    private IEnumerator IncreaseLaserLengthRoutine() {
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            // Обновление размера спрайта
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            // Обновление размера и позиции коллайдера
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearT)) / 2, capsuleCollider2D.offset.y);

            yield return null;
        }

        // Запуск анимации исчезновения
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    // Поворот луча в сторону курсора
    private void LaserFaceMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }
}
