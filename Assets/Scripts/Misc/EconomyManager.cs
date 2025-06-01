using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Класс управления экономикой игры, наследуется от Singleton для глобального доступа
public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;                                  // Текст отображения золота
    private int currentGold = 0;                                // Текущее количество золота

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";         // Имя объекта с текстом золота

    // Обновление количества золота
    public void UpdateCurrentGold() {
        currentGold += 1;

        // Получение компонента текста при необходимости
        if (goldText == null) {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        // Обновление отображения золота
        goldText.text = currentGold.ToString("D3");
    }
}
