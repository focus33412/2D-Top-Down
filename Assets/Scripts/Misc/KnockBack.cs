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
    public void GetKnockedBack(Transform damageSource, float knockBackTrust)
    {
        GettingKnockedBack = true;
        // Расчет силы отбрасывания с учетом массы объекта
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackTrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    // Корутина отбрасывания
    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.linearVelocity = Vector2.zero;                       // Остановка движения
        GettingKnockedBack = false;                             // Сброс флага отбрасывания
    }
}

