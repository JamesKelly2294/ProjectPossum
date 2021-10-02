using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;
    private Animator[] _animators;

    Vector2 _movement;

    private void Start()
    {
        _animators = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        // Input   
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _movement = Vector2.ClampMagnitude(_movement, 1.0f);

        foreach (var animator in _animators)
        {
            animator.SetFloat("Horizontal", _movement.x);
            animator.SetFloat("Vertical", _movement.y);
            animator.SetFloat("Speed", _movement.sqrMagnitude);

            if (_movement.sqrMagnitude > 0.01)
            {
                animator.SetFloat("Horizontal_Facing", _movement.x);
                animator.SetFloat("Vertical_Facing", _movement.y);
            }
        }
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + (_movement * moveSpeed * Time.fixedDeltaTime));
    }
}
