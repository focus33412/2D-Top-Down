using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Класс управления здоровьем врага
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;                  // Максимальное здоровье
    [SerializeField] private GameObject deathVFXPrefab;          // Эффект при смерти
    [SerializeField] private float knockBackTrust = 15f;         // Сила отбрасывания

    public int currentHealth;                                    // Текущее здоровье
    private KnockBack knockBack;                                 // Компонент отбрасывания
    private Flash flash;                                         // Компонент мигания

    // Инициализация компонентов при создании
    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    // Начальная настройка при старте
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    // Получение урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackTrust);
        Debug.Log(currentHealth);
        DetectDeath();
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    // Корутина проверки смерти после мигания
    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMaterialTime());
        DetectDeath();
    }

    // Проверка и обработка смерти врага
    public void DetectDeath()
    {
        if (currentHealth <= 0){
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickUpSpawner>().DropItems();
            GetComponent<Animator>().SetTrigger("Death");
            Destroy(gameObject);
        }
    }
}
