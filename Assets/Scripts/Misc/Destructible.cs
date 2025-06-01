using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Класс разрушаемого объекта
public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destoyVFX;               // Эффект при разрушении

    // Обработка столкновения с источником урона
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Проверка на столкновение с источником урона или снарядом
        if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>()) {
            GetComponent<PickUpSpawner>().DropItems();          // Выпадение предметов
            Instantiate(destoyVFX, transform.position, Quaternion.identity);  // Создание эффекта
            Destroy(gameObject);                                // Уничтожение объекта
        }
    }
}
