using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Класс управления снарядом (стрелой, пулей и т.д.)
public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;         // Скорость движения снаряда
    [SerializeField] private GameObject particleOnHitPrefabVFX;  // Эффект при попадании
    [SerializeField] private bool isEnemyProjectile = false;     // Флаг снаряда врага
    [SerializeField] private float projectileRange = 10f;        // Дальность полета снаряда

    private Vector3 startPosition;                           // Начальная позиция снаряда

    // Сохранение начальной позиции при создании
    private void Start() {
        startPosition = transform.position;
    }

    // Обновление каждый кадр
    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    // Обновление дальности полета снаряда
    public void UpdateProjectileRange(float projectileRange){
        this.projectileRange = projectileRange;
    }

    // Обновление скорости движения снаряда
    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    // Обработка столкновений с другими объектами
    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        // Проверяем столкновение с не-триггер коллайдером
        if (!other.isTrigger && (enemyHealth || indestructible || player)) {
            // Если снаряд врага попал в игрока или снаряд игрока попал во врага
            if ((player && isEnemyProjectile && SceneManager.GetActiveScene().name != "Menu") || (enemyHealth && !isEnemyProjectile)) {
                player?.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            } 
            // Если снаряд попал в неразрушаемый объект
            else if (!other.isTrigger && indestructible) {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    // Проверка дальности полета снаряда
    private void DetectFireDistance() {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange) {
            Destroy(gameObject);
        }
    }

    // Движение снаряда
    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
