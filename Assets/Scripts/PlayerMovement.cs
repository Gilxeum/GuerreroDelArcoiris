using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    Rigidbody2D rb;
    Animator animator;
    BoxCollider2D myFeetCollider;

    Vector2 moveInput;

    float gravityScaleAtStart;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetChild(PlayerPrefs.GetInt("SelectedCharacter",0)).GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rb.gravityScale;
    }

    private void Update()
    {
        Run();
        IsJumping();
        ClimbLadder();
    }

    

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            rb.linearVelocity += new Vector2(0, jumpForce);
        }
      
    }

    private void IsJumping()
    { 
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void Run()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY);


        animator.SetBool("isRunning", moveInput.x != 0);

        if(moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }   

    }

    private void ClimbLadder()
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = gravityScaleAtStart;
            return;
        }

        rb.gravityScale = 0;
        Vector2 climbVelocity = new Vector2(rb.linearVelocity.x, moveInput.y * moveSpeed);
        rb.linearVelocity = climbVelocity;

        animator.SetBool("isClimbing", moveInput.y != 0);

    }





}
