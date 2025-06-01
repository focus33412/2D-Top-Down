using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;

    private bool isShooting = false;
    private EnemyHealth enemyHealth;

    private void Start() {
        enemyHealth = GetComponent<EnemyHealth>();
    }

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

    public void Attack() {
        if (!isShooting) {
            if (enemyHealth != null && enemyHealth.currentHealth <= 4){
                StartCoroutine(EnragedShootRoutine());
            } else {
                StartCoroutine(ShootRoutine());
            }
        }
    }

    private IEnumerator EnragedShootRoutine() {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        // Установка параметров для разъяренной атаки
        float originalAngleSpread = angleSpread;
        int originalBurstCount = burstCount;
        float originalTimeBetweenBursts = timeBetweenBursts;
        float originalRestTime = restTime;
        int originalProjectilesPerBurst = projectilesPerBurst;
        float originalBulletMoveSpeed = bulletMoveSpeed;

        angleSpread = 359f;
        burstCount = 5;
        timeBetweenBursts = 0.3f;
        restTime = 0.5f;
        projectilesPerBurst = 40;
        bulletMoveSpeed = 3f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

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

    private IEnumerator ShootRoutine() {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;//

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 
        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; } 

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

                newBullet.transform.right = dir; // Поворачиваем пулю для визуального эффекта

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

    private Vector2 FindBulletSpawnPos(float currentAngle) {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
