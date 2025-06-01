using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Класс для управления затемнением и проявлением UI-экрана
public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;         // Ссылка на UI-элемент затемнения
    [SerializeField] private float fadeSpeed = 1f;     // Скорость затемнения/проявления

    private IEnumerator fadeRoutine;                   // Ссылка на текущую корутину

    // Запуск затемнения экрана
    public void FadeToBlack() {
        if (fadeRoutine != null) {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }

    // Запуск проявления экрана (убираем затемнение)
    public void FadeToClear() {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }

    // Корутина плавного изменения прозрачности экрана
    private IEnumerator FadeRoutine(float targetAlpha) {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }
}
