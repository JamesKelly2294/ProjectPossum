using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPositionedSprite : MonoBehaviour
{
    [Range(-1.0f, 1.0f)]
    public float Offset = 0.0f;

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + Offset);
    }
}
