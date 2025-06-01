using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // Ссылка на меню паузы
    [SerializeField] private GameObject activeInventory; // Ссылка на активный инвентарь
    [SerializeField] private GameObject uiStats; // Ссылка на UI статистики
    [SerializeField] private GameObject menuUI; // Ссылка на UI меню

    private void Awake()
    {
        // Проверяем наличие EventSystem
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Обновляем видимость UI при загрузке сцены
        UpdateUIVisibility();
    }

    private void UpdateUIVisibility()
    {
        bool isMenuScene = SceneManager.GetActiveScene().name == "Menu";

        // Активируем/деактивируем элементы в зависимости от сцены
        if (pauseMenu != null) pauseMenu.SetActive(!isMenuScene);
        if (activeInventory != null) activeInventory.SetActive(!isMenuScene);
        if (uiStats != null) uiStats.SetActive(!isMenuScene);
        if (menuUI != null) menuUI.SetActive(isMenuScene);

        // Сбрасываем состояние паузы при переходе в меню
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

    public void Play()
    {
        // Загружаем игровую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
