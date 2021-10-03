using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 5.0f;

    [Range(0.1f, 5.0f)]
    public float footstepThreshhold = 1.0f;
    public bool isRunning = false;
    public Rigidbody2D rb;
    private Animator[] _animators;
    private Vector2 _prevPosition;

    Vector2 _movement;

    private void Start()
    {
        _animators = GetComponentsInChildren<Animator>();
    }

    float distanceSinceLastFootstep;

    void Update()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Input   
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _movement = Vector2.ClampMagnitude(_movement, 1.0f);

        foreach (var animator in _animators)
        {
            animator.SetFloat("Horizontal", _movement.x);
            animator.SetFloat("Vertical", _movement.y);
            animator.SetFloat("Speed", _movement.sqrMagnitude);
            animator.SetFloat("AnimationSpeed", isRunning ? 1.5f : 0.8f);

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
        rb.MovePosition(rb.position + (_movement * (isRunning ? runSpeed : walkSpeed) * Time.fixedDeltaTime));
        distanceSinceLastFootstep += (Vector2.Distance(_prevPosition, rb.position));
        
        if (isRunning && distanceSinceLastFootstep > footstepThreshhold)
        {
            AudioManager.Instance.Play("Player/Walk", false, 0.7f, 1.35f, 0.25f, 0.3f);
            distanceSinceLastFootstep = 0.0f;
        }

        _prevPosition = rb.position;
    }
}
