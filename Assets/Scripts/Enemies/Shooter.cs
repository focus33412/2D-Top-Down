using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс стреляющего врага, реализующий интерфейс IEnemy
public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;            // Префаб пули
    [SerializeField] private float bulletMoveSpeed;              // Скорость пули
    [SerializeField] private int burstCount;                     // Количество залпов
    [SerializeField] private int projectilesPerBurst;            // Количество пуль в залпе
    [SerializeField][Range(0, 359)] private float angleSpread;   // Разброс угла стрельбы
    [SerializeField] private float startingDistance = 0.1f;      // Начальная дистанция появления пуль
    [SerializeField] private float timeBetweenBursts;            // Время между залпами
    [SerializeField] private float restTime = 1f;                // Время отдыха между атаками
    [SerializeField] private bool stagger;                       // Флаг последовательной стрельбы
    [SerializeField] private bool oscillate;                     // Флаг колебания направления

    private bool isShooting = false;                            // Флаг стрельбы
    private EnemyHealth enemyHealth;                            // Компонент здоровья

    // Начальная настройка при старте
    private void Start() {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Валидация параметров в инспекторе
    private void OnValidate() {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if (burstCount < 1) { burstCount = 1; }
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }

    // Метод атаки из интерфейса IEnemy
    public void Attack() {
        if (!isShooting) {
            if (enemyHealth != null && enemyHealth.currentHealth <= 4){
                StartCoroutine(EnragedShootRoutine());           // Разъяренная атака при низком здоровье
            } else {
                StartCoroutine(ShootRoutine());                  // Обычная атака
            }
        }
    }

    // Корутина разъяренной атаки
    private IEnumerator EnragedShootRoutine() {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        // Сохранение оригинальных параметров
        float originalAngleSpread = angleSpread;
        int originalBurstCount = burstCount;
        float originalTimeBetweenBursts = timeBetweenBursts;
        float originalRestTime = restTime;
        int originalProjectilesPerBurst = projectilesPerBurst;
        float originalBulletMoveSpeed = bulletMoveSpeed;

        // Установка параметров для разъяренной атаки
        angleSpread = 359f;
        burstCount = 5;
        timeBetweenBursts = 0.3f;
        restTime = 0.5f;
        projectilesPerBurst = 40;
        bulletMoveSpeed = 3f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        // Выполнение разъяренной атаки
        for (int i = 0; i < burstCount; i++) {
            for (int j = 0; j < projectilesPerBurst; j++) {
                Vector2 pos = FindBulletSpawnPos(currentAngle);
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);

                Vector2 dir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                if (rb != null) {
                    rb.linearVelocity = dir * bulletMoveSpeed;
                }

                newBullet.transform.right = dir;

                if (newBullet.TryGetComponent(out Projectile projectile)) {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;
            }

            currentAngle = startAngle;
            yield return new WaitForSeconds(timeBetweenBursts);
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
        }

        // Восстановление оригинальных параметров
        angleSpread = originalAngleSpread;
        burstCount = originalBurstCount;
        timeBetweenBursts = originalTimeBetweenBursts;
        restTime = originalRestTime;
        projectilesPerBurst = originalProjectilesPerBurst;
        bulletMoveSpeed = originalBulletMoveSpeed;

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    // Корутина обычной атаки
    private IEnumerator ShootRoutine() {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 
        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; } 

        // Выполнение обычной атаки
        for (int i = 0; i < burstCount; i++) {
            if (oscillate) { 
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 
            } 

            if (oscillate && i % 2 != 1) {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 
            } else if (oscillate) {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            } 

            for (int j = 0; j < projectilesPerBurst; j++) {
                Vector2 pos = FindBulletSpawnPos(currentAngle);
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);

                Vector2 dir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                if (rb != null) {
                    rb.linearVelocity = dir * bulletMoveSpeed;
                }

                newBullet.transform.right = dir;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); } 
            }

            currentAngle = startAngle;

            yield return new WaitForSeconds(timeBetweenBursts);
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
        }

        if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
        
        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    // Расчет конуса стрельбы
    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle) {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0) {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    // Расчет позиции появления пули
    private Vector2 FindBulletSpawnPos(float currentAngle) {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
