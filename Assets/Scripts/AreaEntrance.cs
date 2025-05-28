using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start() {
        StartCoroutine(InitializeWithDelay());
    }

    private IEnumerator InitializeWithDelay()
    {
        // Даем время на инициализацию всех компонентов
        yield return new WaitForSeconds(0.1f);

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

        if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();
        }
    }
}
