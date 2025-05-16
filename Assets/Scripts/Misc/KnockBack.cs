using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnockBack : MonoBehaviour
{
public bool gettingKnockedBack { get; private set; }

[SerializeField] private float knockBackTime = 0.1f;

private Rigidbody2D rb;
private void Awake(){
    rb = GetComponent<Rigidbody2D>();
}

public void GetKnockedBack(Transform damageSource, float knockBackTrust)
{
    gettingKnockedBack = true;
    Vector2 difference = (transform.position - damageSource.position).normalized * knockBackTrust * rb.mass;
    rb.AddForce(difference, ForceMode2D.Impulse);
    StartCoroutine(KnockRoutine());
}

private IEnumerator KnockRoutine()
{
    yield return new WaitForSeconds(knockBackTime);
    rb.linearVelocity = Vector2.zero;
    gettingKnockedBack = false;
}
}
