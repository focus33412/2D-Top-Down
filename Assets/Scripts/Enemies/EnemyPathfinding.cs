using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс навигации врага
public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;               // Скорость движения

    private Rigidbody2D rb;                                     // Компонент физики
    private Vector2 moveDir;                                     // Направление движения
    private KnockBack knockback;                                 // Компонент отбрасывания
    private SpriteRenderer spriteRenderer;                       // Компонент отрисовки спрайта

    // Инициализация компонентов при создании
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<KnockBack>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Обновление физики каждый фиксированный кадр
    private void FixedUpdate() {
        if (knockback.GettingKnockedBack || rb == null) { return; }           // Пропуск при отбрасывании или отсутствии rb

        Vector2 newPosition = rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Отработка поворота спрайта
        if (moveDir.x < 0) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    // Установка направления движения
    public void MoveTo(Vector2 targetPosition) {
        moveDir = targetPosition;
    }

    // Остановка движения
    public void StopMoving() {
        moveDir = Vector3.zero;
    }

    // Проверка движения
    public bool IsMoving() {
        return moveDir.magnitude > 0.1f;
    }
}
