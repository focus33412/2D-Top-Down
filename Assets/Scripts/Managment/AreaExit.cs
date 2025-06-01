using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Класс управления переходом между зонами
public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;                 // Имя сцены для загрузки
    [SerializeField] private string sceneTransitionName;         // Имя перехода для анимации

    private float waitToLoadTime = 1f;                          // Время ожидания перед загрузкой

    // Обработка входа игрока в зону перехода
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);  // Установка имени перехода
            UIFade.Instance.FadeToBlack();                      // Затемнение экрана
            StartCoroutine(LoadSceneRoutine());                 // Запуск загрузки сцены
        }
    }

    // Корутина загрузки новой сцены
    private IEnumerator LoadSceneRoutine() {
        while (waitToLoadTime >= 0) {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad);                    // Загрузка новой сцены
    } 
}
