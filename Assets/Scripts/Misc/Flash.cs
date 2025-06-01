using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Класс управления эффектом вспышки при получении урона
public class Flash : MonoBehaviour
{
   [SerializeField] private Material whiteFlashMaterial;         // Материал белой вспышки
   [SerializeField] private float restoreDefaultMaterialTime = 0.2f;  // Время восстановления исходного материала

   private Material defaultMaterial;                             // Исходный материал объекта
   private SpriteRenderer spriteRenderer;                        // Компонент отрисовки спрайта

   // Инициализация компонентов при создании
   private void Awake()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
      defaultMaterial = spriteRenderer.material;                 // Сохранение исходного материала
   }

   // Получение времени восстановления материала
   public float GetRestoreMaterialTime()
   {
      return restoreDefaultMaterialTime;
   }

   // Корутина эффекта вспышки
   public IEnumerator FlashRoutine()
   {
        spriteRenderer.material = whiteFlashMaterial;            // Установка материала вспышки
        yield return new WaitForSeconds(restoreDefaultMaterialTime);
        spriteRenderer.material = defaultMaterial;               // Возврат исходного материала
    }
}
