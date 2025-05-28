using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    protected override void Awake()
    {
        base.Awake();
        InitializeCamera();
    }

    private void InitializeCamera()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (cinemachineVirtualCamera == null)
        {
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (cinemachineVirtualCamera == null)
            {
                Debug.LogError("CinemachineVirtualCamera не найден на сцене! Убедитесь, что на сцене есть объект с компонентом CinemachineVirtualCamera");
            }
        }
    }

    public void SetPlayerCameraFollow() {
        if (cinemachineVirtualCamera == null)
        {
            InitializeCamera();
            if (cinemachineVirtualCamera == null)
            {
                Debug.LogError("Не удалось найти CinemachineVirtualCamera!");
                return;
            }
        }
        
        if (PlayerController.Instance == null)
        {
            Debug.LogError("PlayerController.Instance is null!");
            return;
        }

        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
