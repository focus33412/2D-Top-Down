using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс для поворота объекта в сторону курсора мыши
public class MouseFollow : MonoBehaviour
{
    // Обновление каждый кадр
    private void Update() {
        FaceMouse();
    }

    // Поворот объекта в сторону курсора
    private void FaceMouse() {
        // Получение позиции мыши в мировых координатах
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Расчет направления от объекта к курсору
        Vector2 direction = transform.position - mousePosition;

        // Поворот объекта в сторону курсора
        transform.right = -direction;
    }
}
