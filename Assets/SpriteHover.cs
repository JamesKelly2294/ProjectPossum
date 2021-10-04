using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHover : MonoBehaviour
{
    public Vector3 Direction = Vector3.up;
    public float speed = 1.0f;
    public float distance = 1.0f;

    private Vector3 _startOffset;
    private void Start()
    {
        _startOffset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Direction.normalized;
        Vector3 offset = direction * (Mathf.Sin(Time.time * speed) * distance) + (direction * distance / 2.0f);
        transform.position = transform.parent.transform.position + _startOffset + offset;
    }
}
