using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private KnockBack knockBack;   
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }

    private void FixedUpdate() {
        if (knockBack.GettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        if (moveDirection.x < 0 ) {
            spriteRenderer.flipX = true;
        } else if (moveDirection.x > 0) {
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition) {
        moveDirection = targetPosition ;
    }   
}
