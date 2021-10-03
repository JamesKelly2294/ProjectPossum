using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPositionedSprite : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
