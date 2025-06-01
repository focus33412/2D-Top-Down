using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Компонент, отвечающий за спавн подбираемых предметов
public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoinPrefab;    // Префаб золотой монеты
    [SerializeField] private GameObject healthGlobe;       // Префаб глобуса здоровья
    [SerializeField] private GameObject staminaGlobe;      // Префаб глобуса выносливости

    // Спавнит случайный предмет или группу предметов
    public void DropItems() {
        // Генерируем случайное число от 1 до 4
        int randomNum = Random.Range(1, 5);

        // Спавн глобуса здоровья (шанс 25%)
        if (randomNum == 1) {
            Instantiate(healthGlobe, transform.position, Quaternion.identity); 
        } 

        // Спавн глобуса выносливости (шанс 25%)
        if (randomNum == 2) {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity); 
        }

        // Спавн золотых монет (шанс 25%)
        if (randomNum == 3) {
            // Спавним от 1 до 3 монет
            int randomAmountOfGold = Random.Range(1, 4);
            
            for (int i = 0; i < randomAmountOfGold; i++) {
                Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
            }
        }
        // Шанс 25% ничего не спавнить если число 4
    }
}
