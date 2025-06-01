using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс для управления входом в новую зону/локацию
public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName; // Имя перехода для идентификации

    // Запуск инициализации с задержкой
    private void Start() {
        StartCoroutine(InitializeWithDelay());
    }

    // Корутина для инициализации после небольшой задержки
    private IEnumerator InitializeWithDelay()
    {
        // Даем время на инициализацию всех компонентов
        yield return new WaitForSeconds(0.1f);

        // Проверка наличия необходимых синглтонов
        if (SceneManagement.Instance == null) {
            Debug.LogError("SceneManagement.Instance is null!");
            yield break;
        }

        if (PlayerController.Instance == null) {
            Debug.LogError("PlayerController.Instance is null!");
            yield break;
        }

        if (CameraController.Instance == null) {
            Debug.LogError("CameraController.Instance is null!");
            yield break;
        }

        // Если имя перехода совпадает, перемещаем игрока и камеру
        if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();
        }
    }
}
