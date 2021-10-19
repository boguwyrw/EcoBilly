using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    float wateringCanRotationSpeed = 35.0f;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, wateringCanRotationSpeed * Time.deltaTime, 0.0f));
    }
}
