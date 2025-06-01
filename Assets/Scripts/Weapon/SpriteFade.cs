using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс управления затуханием спрайта
public class SpriteFade : MonoBehaviour
{
    [SerializeField] private float fadeTime = .4f;               // Время затухания

    private SpriteRenderer spriteRenderer;                      // Компонент отрисовки спрайта

    // Инициализация компонентов при создании
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Корутина плавного затухания
    public IEnumerator SlowFadeRoutine() {
        float elapsedTime = 0;
        float startValue = spriteRenderer.color.a;

        // Плавное уменьшение прозрачности
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }

        // Уничтожение объекта после затухания
        Destroy(gameObject);
    }
}
