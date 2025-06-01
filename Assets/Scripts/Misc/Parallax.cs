using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс для реализации эффекта параллакса фона
public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxOffset = -0.15f; // Коэффициент смещения для параллакса

    private Camera cam;                // Ссылка на основную камеру
    private Vector2 startPos;          // Начальная позиция объекта
    private Vector2 travel => (Vector2)cam.transform.position - startPos; // Вектор смещения камеры относительно старта

    // Инициализация камеры
    private void Awake() {
        cam = Camera.main;
    }

    // Сохраняем стартовую позицию объекта
    private void Start() {
        startPos = transform.position;
    }

    // Обновляем позицию объекта с учетом параллакса
    private void FixedUpdate() {
        transform.position = startPos + travel * parallaxOffset;
    }
}
