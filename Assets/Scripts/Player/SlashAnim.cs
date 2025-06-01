using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс управления анимацией удара (частицы)
public class SlashAnim : MonoBehaviour
{
    private ParticleSystem ps;                               // Система частиц для эффекта удара

    // Получение компонента системы частиц при создании
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Проверка завершения анимации каждый кадр
    private void Update()
    {
        if (ps && !ps.IsAlive())
        {
            DestroySelf();
        }
    }

    // Уничтожение объекта после завершения анимации
    public void DestroySelf() {
        Destroy(gameObject);
    }
}
