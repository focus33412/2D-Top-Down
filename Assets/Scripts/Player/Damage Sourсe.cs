using UnityEngine;

// Класс источника урона (используется для коллайдеров оружия)
public class DamageSource : MonoBehaviour
{
    private int damageAmount;                                // Количество наносимого урона

    // Получение значения урона из текущего оружия при старте
    private void Start(){
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    // Обработка столкновения с врагом и нанесение урона
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<EnemyHealth>()){
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damageAmount);
        }
    }
}
