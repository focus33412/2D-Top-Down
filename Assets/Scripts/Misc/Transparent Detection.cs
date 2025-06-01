using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Класс для управления прозрачностью объектов при пересечении с игроком
public class TransparentDetection : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f; // Степень прозрачности при пересечении
    [SerializeField] private float fadeTime = .4f;             // Время плавного перехода прозрачности

    private SpriteRenderer spriteRenderer; // Ссылка на компонент SpriteRenderer (если есть)
    private Tilemap tilemap;               // Ссылка на компонент Tilemap (если есть)

    // Инициализация компонентов
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    // Срабатывает при входе другого объекта в триггер
    private void OnTriggerEnter2D(Collider2D other) {
        if (!gameObject.activeInHierarchy) return;
        
        // Проверяем, является ли объект игроком
        if (other.gameObject.GetComponent<PlayerController>()) {
            if (spriteRenderer) {
                // Запускаем корутину плавного изменения прозрачности для SpriteRenderer
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            } else if (tilemap) {
                // Запускаем корутину плавного изменения прозрачности для Tilemap
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }

    // Срабатывает при выходе объекта из триггера
    private void OnTriggerExit2D(Collider2D other) {
        if (!gameObject.activeInHierarchy) return;
        
        // Проверяем, является ли объект игроком
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderer) {
                // Возвращаем прозрачность к исходной (полностью видимый)
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
            } else if (tilemap) {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }
        }
    }

    // Корутина плавного изменения прозрачности для SpriteRenderer
    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency) {
        float elapsedTime = 0;     
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    // Корутина плавного изменения прозрачности для Tilemap
    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}
