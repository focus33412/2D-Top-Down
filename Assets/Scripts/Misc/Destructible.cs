using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destoyVFX;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>()) {
            GetComponent<PickUpSpawner>().DropItems();
            Instantiate(destoyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
