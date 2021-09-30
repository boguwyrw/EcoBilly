using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBilly : MonoBehaviour
{
    float billyWalkingSpeed = 4.0f;
    float billyRotationSpeed = 130.0f;
    float billysAngle = 0.0f;
    bool turnedAround = false;
    Animator billyAnimator;
    Vector3 velocity;

    void Start()
    {
        billyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameController.Instance.startGame)
        {
            BillysMovement();

            BillysRotation();

            SetBillysAngles();
        }

        BillysAnimation();
    }

    void BillysMovement()
    {
        velocity = Vector3.forward * billyWalkingSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    void BillysRotation()
    {
        if (billysAngle > 80 && GameController.Instance.direction == -1)
        {
            GameController.Instance.direction = 1;
            turnedAround = true;
        }
        else if (billysAngle > 80 && GameController.Instance.direction == 1)
        {
            GameController.Instance.direction = -1;
            turnedAround = true;
        }
        
        if (billysAngle < 1 && turnedAround)
        {
            GameController.Instance.direction = 0;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0.0f, transform.localEulerAngles.z);
            turnedAround = false;
        }

        transform.Rotate(Vector3.up, billyRotationSpeed * GameController.Instance.direction * Time.deltaTime);
    }

    void BillysAnimation()
    {
        billyAnimator.SetFloat("Speed", velocity.z);
    }

    void SetBillysAngles()
    {
        if (transform.localEulerAngles.y < 360 && transform.localEulerAngles.y >= 250)
        {
            billysAngle = 360 - transform.localEulerAngles.y;
        }
        else if (transform.localEulerAngles.y > 0 && transform.localEulerAngles.y <= 110)
        {
            billysAngle = transform.localEulerAngles.y;
        }
    }
}
