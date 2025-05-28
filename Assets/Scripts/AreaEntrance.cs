using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private IEnumerator Start()
    {
        // Ждём, пока загрузится PlayerController и SceneManagement
        yield return new WaitUntil(() => SceneManagement.Instance != null && PlayerController.Instance != null);

        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;

            // Безопасный вызов установки камеры
            if (CameraController.Instance != null)
            {
                CameraController.Instance.SetPlayerCameraFollow();
            }
        }
    }
}
