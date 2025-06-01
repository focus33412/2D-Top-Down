using UnityEngine;
using UnityEngine.SceneManagement;

// Класс управления меню паузы
public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;                                       // Флаг состояния паузы
    public GameObject pauseMenu;                                 // Панель меню паузы

    // Инициализация при создании объекта
    private void Awake()
    {
        // Подписка на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Скрытие панели паузы при создании
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    // Отписка от событий при уничтожении объекта
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Обработка загрузки новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Скрытие панели паузы и сброс времени
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        PauseGame = false;
    }

    // Начальная настройка при старте
    private void Start()
    {
        // Скрытие панели паузы
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    // Обработка активации объекта
    private void OnEnable()
    {
        // Скрытие панели паузы
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    // Обновление каждый кадр
    void Update() {
        // Проверка нажатия клавиши Escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (PauseGame) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    // Возобновление игры
    public void Resume() {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        PauseGame = false;
    }

    // Постановка игры на паузу
    public void Pause() {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
        Time.timeScale = 0f;
        PauseGame = true;
    }  

    // Загрузка главного меню
    public void LoadMenu() {
        // Скрытие панели паузы и сброс времени
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        PauseGame = false;
        
        // Загрузка сцены меню
        SceneManager.LoadScene("Menu");
    }
}
