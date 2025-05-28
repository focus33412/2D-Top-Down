using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        StartCoroutine(WaitAndAttachCamera());
    }

    private IEnumerator WaitAndAttachCamera()
    {
        // Ждём пока PlayerController будет доступен
        while (PlayerController.Instance == null)
        {
            yield return null;
        }

        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        if (cinemachineVirtualCamera != null)
        {
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
        }
        else
        {
            Debug.LogWarning("CinemachineVirtualCamera not found!");
        }
    }
}
