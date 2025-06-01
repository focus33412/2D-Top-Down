using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Класс для управления эффектом тряски экрана
public class ScreenShakeManager : Singleton<ScreenShakeManager>
{
    private CinemachineImpulseSource source; // Источник импульса для тряски

    // Инициализация компонента импульса
    protected override void Awake() {
        base.Awake();
        source = GetComponent<CinemachineImpulseSource>();
    }

    // Метод для вызова тряски экрана
    public void ShakeScreen() {
        source.GenerateImpulse();
    }
}
