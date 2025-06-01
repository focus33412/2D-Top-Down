using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Базовый класс для реализации паттерна Singleton в Unity
public class Singleton<T> : MonoBehaviour where T : Singleton<T> 
{
    private static T instance;                                // Единственный экземпляр класса
    public static T Instance { get { return instance; } }     // Публичный доступ к экземпляру

    // Инициализация при создании объекта
    protected virtual void Awake() {
        // Проверка на существование другого экземпляра
        if (instance != null && this.gameObject != null) {
            Destroy(this.gameObject);
        } else {
            instance = (T)this;
        }

        // Сохранение объекта между сценами, если он не является дочерним
        if (!gameObject.transform.parent) {
            DontDestroyOnLoad(gameObject);
        }
    }
}
