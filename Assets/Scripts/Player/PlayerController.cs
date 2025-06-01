using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : Singleton<PlayerController>
{

    

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] float dashClipLength = 0.367f;
    [SerializeField] private float dashCooldown = 0.7f;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    public PlayerControls Controls => playerControls;

    private Vector2 movement;
    private Vector2 dashDirection;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private KnockBack knockback;
    private Animator animator;
    private const string moveX = "Horizontal";  
    private const string moveY = "Vertical";     
    private const string SPEED = "Speed";
    
    private bool isDashing = false;

    private float originalSpeed;

    


    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls();
        knockback = GetComponent<KnockBack>();
    }

    private void Start() {
        originalSpeed = moveSpeed;
        playerControls.Player.Sprint.performed += _ => Dash();
        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Update() {
        PlayerInput();
        Flip();

        if (!isDashing) {
            Animate();
        }

    }

    private void FixedUpdate() {
        Vector2 movement = new Vector2(playerControls.Player.Move.ReadValue<Vector2>().x, playerControls.Player.Move.ReadValue<Vector2>().y);
        Move(movement);
    }

    public Transform GetWeaponCollider() {
        return weaponCollider;
    }

    private void PlayerInput() {
        float moveX = playerControls.Player.Move.ReadValue<Vector2>().x;
        float moveY = playerControls.Player.Move.ReadValue<Vector2>().y;    

        movement = new Vector2(moveX, moveY);
    
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        movement.Normalize();
    }

    private void Move(Vector2 movement) {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    void Animate() {
        if (animator != null) {
            animator.SetFloat(moveX, movement.x);
            animator.SetFloat(moveY, movement.y);
            animator.SetFloat(SPEED, movement.sqrMagnitude);
        }
    }

    private void Flip() {
        if (movement.x > 0) {
            spriteRenderer.flipX = false;
        }
        
    }

    private void Dash() {
        if (!isDashing && movement != Vector2.zero && Stamina.Instance.CurrentStamina > 0) {
            Stamina.Instance.UseStamina();
            knockback.GetKnockedBack(transform, 10f);
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine() {
        isDashing = true;
        dashDirection = movement;
        moveSpeed = dashSpeed;
        if (dashDirection != Vector2.zero) {
            animator.SetFloat(moveX, dashDirection.x);
            animator.SetFloat(moveY, dashDirection.y);
            animator.SetBool("IsDashing", true);
            yield return new WaitForSeconds(dashClipLength);
        }

        moveSpeed = originalSpeed;
        animator.SetBool("IsDashing", false);

        yield return new WaitForSeconds(dashCooldown);

        isDashing = false;
    }
}
