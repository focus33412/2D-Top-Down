using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Класс для управления виртуальной камерой и следованием за игроком
public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera; // Ссылка на виртуальную камеру

    // Вызывается при старте сцены
    private void Start() {
        SetPlayerCameraFollow();
    }

    // Устанавливает слежение камеры за игроком
    public void SetPlayerCameraFollow() {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
