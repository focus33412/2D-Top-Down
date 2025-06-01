using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Класс, содержащий информацию о враге для спавна
[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;    // Префаб врага для спавна
    public float spawnChance;         // Шанс появления врага (от 0 до 1)
    public int maxEnemiesOnScene;     // Максимальное количество врагов данного типа на сцене
}

// Компонент, отвечающий за спавн врагов в игре
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnInfo> enemiesToSpawn;     // Список врагов для спавна
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 10f);    // Размер области спавна
    [SerializeField] private float spawnCheckInterval = 2f;           // Интервал проверки необходимости спавна
    [SerializeField] private int maxTotalEnemies = 20;                // Максимальное общее количество врагов
    [SerializeField] private bool avoidCameraView = true;             // Избегать спавна в поле зрения камеры
    [SerializeField] private float cameraViewBuffer = 2f;             // Буфер вокруг видимой области камеры
    [SerializeField] private Camera targetCamera;                     // Целевая камера для проверки видимости
    
    private List<GameObject> activeEnemies = new List<GameObject>();  // Список активных врагов
    private float nextSpawnCheck;                                     // Время следующей проверки спавна

    // Инициализация компонента
    private void Start()
    {
        nextSpawnCheck = Time.time + spawnCheckInterval;
    }

    // Проверка необходимости спавна врагов каждый кадр
    private void Update()
    {
        if (Time.time >= nextSpawnCheck)
        {
            CheckAndSpawnEnemies();
            nextSpawnCheck = Time.time + spawnCheckInterval;
        }
    }

    // Проверяет и спавнит врагов при необходимости
    private void CheckAndSpawnEnemies()
    {
        // Удаляем уничтоженных врагов из списка
        activeEnemies.RemoveAll(enemy => enemy == null);

        // Проверяем, не превышен ли лимит врагов
        if (activeEnemies.Count >= maxTotalEnemies)
            return;

        // Проверяем каждый тип врага
        foreach (var enemyInfo in enemiesToSpawn)
        {
            int currentEnemyCount = activeEnemies.Count(e => e.name.Contains(enemyInfo.enemyPrefab.name));
            
            // Если количество врагов данного типа меньше максимального
            if (currentEnemyCount < enemyInfo.maxEnemiesOnScene)
            {
                // Проверяем шанс появления
                if (Random.value <= enemyInfo.spawnChance)
                {
                    Vector2 spawnPosition = GetRandomPositionInSpawnArea();
                    // Проверяем, не находится ли позиция в поле зрения камеры
                    if (!avoidCameraView || !IsPositionInCameraView(spawnPosition))
                    {
                        SpawnEnemy(enemyInfo.enemyPrefab, spawnPosition);
                    }
                }
            }
        }
    }

    // Проверяет, находится ли позиция в поле зрения камеры
    private bool IsPositionInCameraView(Vector2 position)
    {
        if (targetCamera == null) return false;

        Vector3 viewportPoint = targetCamera.WorldToViewportPoint(position);
        
        // Добавляем буфер вокруг видимой области
        float buffer = cameraViewBuffer / targetCamera.orthographicSize;
        
        return viewportPoint.x > -buffer && 
               viewportPoint.x < 1 + buffer && 
               viewportPoint.y > -buffer && 
               viewportPoint.y < 1 + buffer;
    }

    // Создает врага в указанной позиции
    private void SpawnEnemy(GameObject enemyPrefab, Vector2 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        activeEnemies.Add(enemy);
    }

    // Получает случайную позицию в области спавна
    private Vector2 GetRandomPositionInSpawnArea()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        return (Vector2)transform.position + new Vector2(randomX, randomY);
    }

    // Отрисовывает область спавна в редакторе Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0));
    }
} 