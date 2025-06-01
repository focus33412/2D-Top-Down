using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Класс управления главным меню
public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;               // Ссылка на меню паузы
    [SerializeField] private GameObject activeInventory;         // Ссылка на активный инвентарь
    [SerializeField] private GameObject uiStats;                 // Ссылка на UI статистики
    [SerializeField] private GameObject menuUI;                  // Ссылка на UI меню

    // Инициализация при создании объекта
    private void Awake()
    {
        // Проверка и создание EventSystem при необходимости
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Подписка на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Отписка от событий при уничтожении объекта
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Обработка загрузки новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Обновление видимости UI элементов
        UpdateUIVisibility();
    }

    // Обновление видимости UI элементов
    private void UpdateUIVisibility()
    {
        bool isMenuScene = SceneManager.GetActiveScene().name == "Menu";

        // Управление видимостью UI элементов в зависимости от сцены
        if (pauseMenu != null) pauseMenu.SetActive(!isMenuScene);
        if (activeInventory != null) activeInventory.SetActive(!isMenuScene);
        if (uiStats != null) uiStats.SetActive(!isMenuScene);
        if (menuUI != null) menuUI.SetActive(isMenuScene);

        // Сброс состояния паузы при переходе в меню
        if (isMenuScene)
        {
            Time.timeScale = 1f;
            if (pauseMenu != null)
            {
                PauseMenu pauseMenuComponent = pauseMenu.GetComponent<PauseMenu>();
                if (pauseMenuComponent != null)
                {
                    pauseMenuComponent.PauseGame = false;
                }
            }
        }
    }

    // Запуск игры
    public void Play()
    {
        // Загрузка следующей сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Выход из игры
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
