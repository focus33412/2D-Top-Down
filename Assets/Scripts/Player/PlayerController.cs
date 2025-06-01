using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

// Основной класс управления игроком, наследуется от Singleton для глобального доступа
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 5f;               // Скорость движения персонажа
    [SerializeField] private float dashSpeed = 10f;              // Скорость рывка
    [SerializeField] private float dashDuration = 0.2f;          // Длительность рывка
    [SerializeField] private float dashCooldown = 1f;            // Время перезарядки рывка
    [SerializeField] private Transform weaponCollider;      // Коллайдер оружия

    private PlayerControls playerControls;                  // Система управления
    public PlayerControls Controls => playerControls;       // Публичный доступ к контролам

    private Vector2 moveDirection;                               // Направление движения
    private Rigidbody2D rb;                                      // Компонент физики
    private SpriteRenderer spriteRenderer;                  // Рендерер спрайта
    private KnockBack knockback;                            // Компонент отбрасывания
    private Animator animator;                              // Компонент анимации
    private const string moveX = "Horizontal";              // Параметр анимации движения по X
    private const string moveY = "Vertical";                // Параметр анимации движения по Y
    private const string SPEED = "Speed";                   // Параметр анимации скорости
    
    private bool isDashing;                                      // Флаг выполнения рывка
    private bool canDash = true;                                 // Флаг доступности рывка
    private float originalSpeed;                            // Сохранение оригинальной скорости

    // Инициализация компонентов при создании объекта
    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls();
        knockback = GetComponent<KnockBack>();
    }

    // Начальная настройка при старте
    private void Start() {
        originalSpeed = moveSpeed;
        playerControls.Player.Sprint.performed += _ => Dash();
        ActiveInventory.Instance.EquipStartingWeapon();
    }

    // Включение системы управления
    private void OnEnable() {
        playerControls.Enable();
    }

    // Отключение системы управления
    private void OnDisable() {
        playerControls.Disable();
    }

    // Обновление каждый кадр
    private void Update() {
        // Получение входных данных
        float moveX = playerControls.Player.Move.ReadValue<Vector2>().x;
        float moveY = playerControls.Player.Move.ReadValue<Vector2>().y;
        moveDirection = new Vector2(moveX, moveY).normalized;

        // Проверка нажатия клавиши рывка
        if (playerControls.Player.Sprint.WasPressedThisFrame() && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();

        if (!isDashing) {
            Animate();
        }
    }

    // Физическое обновление движения
    private void FixedUpdate() {
        if (!isDashing) {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    // Получение коллайдера оружия
    public Transform GetWeaponCollider() {
        return weaponCollider;
    }

    // Обновление анимации движения
    void Animate() {
        if (animator != null) {
            animator.SetFloat(moveX, moveDirection.x);
            animator.SetFloat(moveY, moveDirection.y);
            animator.SetFloat(SPEED, moveDirection.sqrMagnitude);
        }
    }

    // Отражение спрайта по горизонтали
    private void Flip() {
        if (moveDirection.x > 0) {
            spriteRenderer.flipX = false;
        }
    }

    // Корутина выполнения рывка
    private IEnumerator Dash() {
        isDashing = true;
        canDash = false;
        rb.linearVelocity = moveDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
