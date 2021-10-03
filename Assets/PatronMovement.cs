using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;
    public bool loop = true;
    public bool pingPong = true;
    public bool followingPath = true;
    public bool movingForward = true;
    public List<Transform> path;

    public int pathIndex = 0;
    private Animator[] _animators;

    Vector2 _movement;

    private void Start()
    {
        _animators = GetComponentsInChildren<Animator>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked!");
    }

    void Update()
    {
        if (followingPath)
        {
            Vector2 destination = path[pathIndex].position;
            Vector2 origin = transform.position;
            if (Vector2.Distance(destination, origin) < 0.1)
            {
                if (movingForward)
                {
                    pathIndex += 1;

                    if (pathIndex >= path.Count)
                    {
                        if (!loop)
                        {
                            followingPath = false;
                            pathIndex -= 1;
                            return;
                        }
                        else
                        {
                            if (pingPong)
                            {
                                movingForward = false;
                                pathIndex -= 2;
                            }
                            else
                            {
                                pathIndex = 0;
                            }
                        }
                    }
                }
                else
                {
                    pathIndex -= 1;


                    if (pathIndex < 0)
                    {
                        if (!loop)
                        {
                            followingPath = false;
                            pathIndex += 1;
                            return;
                        }
                        else
                        {

                            if (pingPong)
                            {
                                movingForward = true;
                                pathIndex += 2;
                            }
                            else
                            {
                                pathIndex = path.Count - 1;
                            }
                        }
                    }
                }

                destination = path[pathIndex].position;
            }

            Vector2 pathDir = (destination - origin).normalized;
            _movement = pathDir;
        } else
        {
            _movement = Vector2.zero;
        }


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
