using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBag : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
}
