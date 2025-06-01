using System.Collections;   
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyPathfinding enemyPathfinding;
    private const string SPEED = "Speed";

    void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    void Animate() {
        if (animator != null) {
            // Проверяем, движется ли краб, используя moveDir из EnemyPathfinding
            bool isMoving = enemyPathfinding != null && enemyPathfinding.IsMoving();
            
            if (isMoving)
            {
                animator.SetFloat(SPEED, 1f);
            }
            else
            {
                animator.SetFloat(SPEED, 0f);
            }
        }
    }

    void Update() {
        Animate();
    }
}

