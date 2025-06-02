using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Класс управления отбрасыванием объектов
public class KnockBack : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }        // Флаг состояния отбрасывания

    [SerializeField] private float knockBackTime = 0.1f;        // Время отбрасывания

    private Rigidbody2D rb;                                     // Компонент физики

    // Инициализация компонентов при создании
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Получение отбрасывания от источника урона
    public void GetKnockedBack(Transform damageSource, float knockbackPower)
    {
        if (rb == null) return;
        
        StopAllCoroutines();
        StartCoroutine(KnockRoutine(damageSource, knockbackPower));
    }

    // Корутина отбрасывания
    private IEnumerator KnockRoutine(Transform damageSource, float knockbackPower)
    {
        GettingKnockedBack = true;
        
        // Вычисляем направление отбрасывания
        Vector2 direction = (transform.position - damageSource.position).normalized;
        
        // Применяем начальную скорость
        float currentSpeed = knockbackPower;
        rb.velocity = direction * currentSpeed;
        
        // Плавно уменьшаем скорость
        float elapsedTime = 0f;
        while (elapsedTime < knockBackTime) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / knockBackTime;
            currentSpeed = Mathf.Lerp(knockbackPower, 0f, t);
            rb.velocity = direction * currentSpeed;
            yield return null;
        }
        
        // Сбрасываем скорость в конце
        rb.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}

