using System.Collections;   
using System.Collections.Generic;
using UnityEngine;

// Класс управления крабом
public class Crab : MonoBehaviour
{
    private Animator animator;                                   // Компонент аниматора
    private Rigidbody2D rb;                                     // Компонент физики
    private EnemyPathfinding enemyPathfinding;                  // Компонент навигации
    private const string SPEED = "Speed";                       // Параметр скорости для аниматора

    // Инициализация компонентов при создании
    void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    // Управление анимацией
    void Animate() {
        if (animator != null) {
            // Проверка движения краба через компонент навигации
            bool isMoving = enemyPathfinding != null && enemyPathfinding.IsMoving();
            
            if (isMoving)
            {
                animator.SetFloat(SPEED, 1f);                   // Анимация движения
            }
            else
            {
                animator.SetFloat(SPEED, 0f);                   // Анимация покоя
            }
        }
    }

    // Обновление каждый кадр
    void Update() {
        Animate();
    }
}

