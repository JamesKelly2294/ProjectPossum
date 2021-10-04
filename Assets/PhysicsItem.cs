using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsItem : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public GameObject Target;
    public float AttractionStrength = 35.0f;
    public float AcquiredDistance = 0.25f;
    public float MaxDistance = 2.5f;
    public float MaxSpeed = 15.0f;

    // Update is called once per frame
    void Update()
    {
        //get the offset between the objects
        Vector3 offset = Target.transform.position - transform.position;
        offset.z = 0;
        
        float magsqr = offset.sqrMagnitude;

        if (magsqr > (MaxDistance * MaxDistance))
        {
            return;
        }

        if (magsqr < (AcquiredDistance * AcquiredDistance))
        {
            AudioManager.Instance.Play("SFX/Item/PickUp", false, 0.6f, 1.0f, 0.1f, 0.25f);
            gameObject.SetActive(false);
            //Destroy(gameObject);
            return;
        }

        float curSpeed = (1.0f / offset.sqrMagnitude) * AttractionStrength;
        Rigidbody2D.MovePosition(Rigidbody2D.position + new Vector2(offset.normalized.x, offset.normalized.y) * curSpeed * Time.deltaTime);
    }
}
