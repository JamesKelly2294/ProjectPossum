using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    // Start is called before the first frame update
    void Start()
    {
        // Very professional
        var animators = GetComponentsInChildren<Animator>();
        Vector2 facing = Vector2.right;
        foreach (var animator in animators)
        {
            animator.SetFloat("Horizontal_Facing", facing.x);
            animator.SetFloat("Vertical_Facing", facing.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
