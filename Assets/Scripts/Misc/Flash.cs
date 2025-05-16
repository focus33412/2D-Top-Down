using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flash : MonoBehaviour
{
   [SerializeField] private Material whiteFlashMaterial;
   [SerializeField] private float restoreDefaultMaterialTime = 0.2f;


   private Material defaultMaterial;
   private SpriteRenderer spriteRenderer;
   private EnemyHealth enemyHealth;

   private void Awake()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
      enemyHealth = GetComponent<EnemyHealth>();
      defaultMaterial = spriteRenderer.material;
      
   }

   public IEnumerator FlashRoutine()
   {
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(restoreDefaultMaterialTime);
        spriteRenderer.material = defaultMaterial;
        enemyHealth.DetectDeath();
    }
   
   
   
}
