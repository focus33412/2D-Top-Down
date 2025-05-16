using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 3f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private const string moveX = "Horizontal";  
    private const string moveY = "Vertical";     
    private const string SPEED = "Speed";
    


    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        Animate();
        PlayerInput();
        Flip();

    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(playerControls.Player.Move.ReadValue<Vector2>().x, playerControls.Player.Move.ReadValue<Vector2>().y);
        
        Move(movement);
    }

    private void PlayerInput()
    {
        float moveX = playerControls.Player.Move.ReadValue<Vector2>().x;
        float moveY = playerControls.Player.Move.ReadValue<Vector2>().y;    

        movement = new Vector2(moveX, moveY);
    
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        movement.Normalize();
    }

    private void Move(Vector2 movement)
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    void Animate()
    {
        if (animator != null)
        {
            animator.SetFloat(moveX, movement.x);
            animator.SetFloat(moveY, movement.y);
            animator.SetFloat(SPEED, movement.sqrMagnitude);
        }
    }

    private void Flip()
    {
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
            
        }
        
    }

    
}
