using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    
    void Update()
    {
        // Input   
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = Vector2.ClampMagnitude(movement, 1.0f);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        
        if (movement.sqrMagnitude > 0.01)
        {
            animator.SetFloat("Horizontal_Facing", movement.x);
            animator.SetFloat("Vertical_Facing", movement.y);
        }
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + (movement * moveSpeed * Time.fixedDeltaTime));
    }
}
