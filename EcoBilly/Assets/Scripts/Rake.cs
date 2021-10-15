using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rake : MonoBehaviour
{
    float rakeRotationSpeed = 35.0f;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, rakeRotationSpeed * Time.deltaTime, 0.0f));
    }
}
