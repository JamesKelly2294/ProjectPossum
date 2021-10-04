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
    public Direction FacingDirection = Direction.South;
    public Vector2 FacingVector = Vector2.down;
    private Animator[] _animators;
    private Vector2 _prevPosition;
    
    Vector2 _movement;

    private void Start()
    {
        _animators = GetComponentsInChildren<Animator>();
    }

    float distanceSinceLastFootstep;

    void SetFacing(Vector2 facing, bool updateDirection=true)
    {
        foreach (var animator in _animators)
        {
            animator.SetFloat("Horizontal_Facing", facing.x);
            animator.SetFloat("Vertical_Facing", facing.y);
        }

        if (updateDirection)
        {
            if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
            {
                if (_movement.x < 0)
                {
                    FacingDirection = Direction.West;
                }
                else
                {
                    FacingDirection = Direction.East;
                }
            }
            else
            {
                if (_movement.y < 0)
                {
                    FacingDirection = Direction.South;
                }
                else
                {
                    FacingDirection = Direction.North;
                }
            }
        }
    }

    public void FaceDirection(Direction direction)
    {
        FacingDirection = direction;
        FacingVector = CalculateOffsetVector2IntForFacing(direction);

        SetFacing(FacingVector, updateDirection: false);
    }

    public Vector2Int CalculateOffsetVector2IntForFacing(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2Int(0, 1);
            case Direction.East:
                return new Vector2Int(1, 0);
            case Direction.South:
                return new Vector2Int(0, -1);
            case Direction.West:
                return new Vector2Int(-1, 0);
            default:
                return Vector2Int.down;
        }
    }

    void Update()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (CharacterDialog.Instance.ActiveScript != null)
        {
            _movement = Vector2.zero;
        } else
        {
            // Input   
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        _movement = Vector2.ClampMagnitude(_movement, 1.0f);

        foreach (var animator in _animators)
        {
            animator.SetFloat("Horizontal", _movement.x);
            animator.SetFloat("Vertical", _movement.y);
            animator.SetFloat("Speed", _movement.sqrMagnitude);
            animator.SetFloat("AnimationSpeed", isRunning ? 1.5f : 0.8f);

            if (_movement.sqrMagnitude > 0.01)
            {
                SetFacing(_movement);
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
