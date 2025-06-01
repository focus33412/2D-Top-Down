using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public float spawnChance;
    public int maxEnemiesOnScene;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnInfo> enemiesToSpawn;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 10f);
    [SerializeField] private float spawnCheckInterval = 2f;
    [SerializeField] private int maxTotalEnemies = 20;
    [SerializeField] private bool avoidCameraView = true;
    [SerializeField] private float cameraViewBuffer = 2f;
    [SerializeField] private Camera targetCamera; // Целевая камера для проверки видимости
    
    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnCheck;

    private void Start()
    {
        nextSpawnCheck = Time.time + spawnCheckInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnCheck)
        {
            CheckAndSpawnEnemies();
            nextSpawnCheck = Time.time + spawnCheckInterval;
        }
    }

    private void CheckAndSpawnEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (activeEnemies.Count >= maxTotalEnemies)
            return;

        foreach (var enemyInfo in enemiesToSpawn)
        {
            int currentEnemyCount = activeEnemies.Count(e => e.name.Contains(enemyInfo.enemyPrefab.name));
            
            if (currentEnemyCount < enemyInfo.maxEnemiesOnScene)
            {
                if (Random.value <= enemyInfo.spawnChance)
                {
                    Vector2 spawnPosition = GetRandomPositionInSpawnArea();
                    if (!avoidCameraView || !IsPositionInCameraView(spawnPosition))
                    {
                        SpawnEnemy(enemyInfo.enemyPrefab, spawnPosition);
                    }
                }
            }
        }
    }

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

    private void SpawnEnemy(GameObject enemyPrefab, Vector2 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        activeEnemies.Add(enemy);
    }

    private Vector2 GetRandomPositionInSpawnArea()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        return (Vector2)transform.position + new Vector2(randomX, randomY);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0));
    }
} 