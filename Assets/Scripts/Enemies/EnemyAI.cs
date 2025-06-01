using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс управления ИИ врага
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;      // Время до смены направления блуждания
    [SerializeField] private float attackRange = 0f;             // Дальность атаки
    [SerializeField] private MonoBehaviour enemyType;            // Тип врага (реализующий IEnemy)
    [SerializeField] private float attackCooldown = 2f;          // Время перезарядки атаки
    [SerializeField] private bool stopMovingWhileAttacking = false;  // Остановка при атаке

    private bool canAttack = true;                              // Флаг возможности атаки

    // Состояния врага
    private enum State {
        Roaming,    // Блуждание
        Attacking   // Атака
    }

    private Vector2 roamPosition;                                // Позиция для блуждания
    private float timeRoaming = 0f;                             // Время в текущем направлении
    private State state;                                        // Текущее состояние
    private EnemyPathfinding enemyPathfinding;                  // Компонент навигации

    // Инициализация компонентов при создании
    private void Awake() {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    // Начальная настройка при старте
    private void Start() {
        roamPosition = GetRoamingPosition();
    }

    // Обновление каждый кадр
    private void Update() {
        MovementStateControl();
    }

    // Управление состояниями движения
    private void MovementStateControl() {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;
        }
    }

    // Логика блуждания
    private void Roaming() {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);

        // Проверка дистанции до игрока для перехода в атаку
        if (PlayerController.Instance != null && 
            Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange) {
            state = State.Attacking;
        }

        // Смена направления блуждания
        if (timeRoaming > roamChangeDirFloat) {
            roamPosition = GetRoamingPosition();
        }
    }

    // Логика атаки
    private void Attacking() {
        // Проверка дистанции до игрока для возврата к блужданию
        if (PlayerController.Instance != null && 
            Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        // Выполнение атаки
        if (attackRange != 0 && canAttack) {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            // Остановка или продолжение движения во время атаки
            if (stopMovingWhileAttacking) {
                enemyPathfinding.StopMoving();
            } else {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    // Корутина перезарядки атаки
    private IEnumerator AttackCooldownRoutine() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Получение случайной позиции для блуждания
    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
