using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackTrust = 15f;

    public int currentHealth;
    private KnockBack knockBack;
    private Flash flash;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackTrust);
        Debug.Log(currentHealth);
        DetectDeath();
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMaterialTime());
        DetectDeath();
    }

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
