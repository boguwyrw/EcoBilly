using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerBilly;

    Vector3 offset;

    void Start()
    {
        offset = playerBilly.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = playerBilly.position - offset;
    }
}
