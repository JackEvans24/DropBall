using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private float speed;

    void FixedUpdate()
    {
        if (transform.rotation.z > 360)
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z - 360);

        transform.Rotate(new Vector3(0, 0, speed));
    }
}
