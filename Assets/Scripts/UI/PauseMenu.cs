using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseMenu;

    private void Awake()
    {
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Скрываем панель паузы при создании объекта
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Принудительно скрываем панель паузы при загрузке любой сцены
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        PauseGame = false;
    }

    private void Start()
    {
        // Скрываем панель паузы при старте
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void OnEnable()
    {
        // Скрываем панель паузы при активации объекта
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (PauseGame) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        PauseGame = false;
    }

    public void Pause() {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
        Time.timeScale = 0f;
        PauseGame = true;
    }  

    public void LoadMenu() {
        // Скрываем панель паузы
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        PauseGame = false;
        
        // Загружаем меню
        SceneManager.LoadScene("Menu");
    }
}
