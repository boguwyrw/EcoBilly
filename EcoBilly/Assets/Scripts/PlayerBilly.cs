using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBilly : MonoBehaviour
{
    float stage_1_speed = 6.0f;
    float stage_2_speed = 7.5f;
    float stage_3_speed = 9.0f;
    float billyWalkingSpeed = 0.0f;
    int maxDamages = 5;
    /*
    float billyRotationSpeed = 160.0f;
    float billysAngle = 0.0f;
    float maxAngle = 60.0f;
    float minAngle = 1.5f;
    bool turnedAround = false;
    */
    Animator billyAnimator;
    Vector3 velocity;
    Vector3 startPosition;
    Vector3 restartPosition;

    void Start()
    {
        billyWalkingSpeed = stage_1_speed;

        billyAnimator = GetComponent<Animator>();

        startPosition = transform.position;
        restartPosition = startPosition;
    }

    void FixedUpdate()
    {
        if (GameController.Instance.startGame)
        {
            BillysMovement();

            //SetBillysAngles();

            //BillysRotation();
        }

        BillysAnimation();
    }

    void BillysMovement()
    {
        velocity = Vector3.forward * billyWalkingSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    /*
    void SetBillysAngles()
    {
        if (transform.localEulerAngles.y < 360 && transform.localEulerAngles.y >= 270)
        {
            billysAngle = 360 - transform.localEulerAngles.y;
        }
        else if (transform.localEulerAngles.y > 0 && transform.localEulerAngles.y <= 90)
        {
            billysAngle = transform.localEulerAngles.y;
        }
    }

    void BillysRotation()
    {
        if ((GameController.Instance.direction == 1 && billysAngle > minAngle) || (GameController.Instance.direction == -1 && billysAngle > minAngle))
        {
            turnedAround = true;
        }

        SetStraightPosition();

        if (billysAngle > maxAngle && turnedAround)
        {
            if ((transform.localEulerAngles.y < (360.0f - maxAngle)) && (transform.localEulerAngles.y > (maxAngle + 20.0f)))
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 360.0f - maxAngle, transform.localEulerAngles.z);
            }
            else if (transform.localEulerAngles.y > maxAngle)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, maxAngle, transform.localEulerAngles.z);
            }
        }

        transform.Rotate(Vector3.up, billyRotationSpeed * GameController.Instance.direction * Time.deltaTime);
    }

    void SetStraightPosition()
    {
        if (billysAngle < minAngle && turnedAround)
        {
            GameController.Instance.direction = 0;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0.0f, transform.localEulerAngles.z);
            billysAngle = 0.0f;
            turnedAround = false;
        }
    }
    */

    void BillysAnimation()
    {
        billyAnimator.SetFloat("Speed", velocity.z);
    }

    void BackToCheckpointPosition()
    {
        GameController.Instance.damages = 0;
        transform.position = restartPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            GameController.Instance.damages += 1;
            if (GameController.Instance.damages == maxDamages)
            {
                BackToCheckpointPosition();
            }
        }

        if (collision.gameObject.layer == 7)
        {
            GameController.Instance.points += 1;
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            float positionZ = other.gameObject.GetComponent<BoxCollider>().center.z;
            restartPosition = new Vector3(transform.position.x, transform.position.y, positionZ);
        }

        if (other.gameObject.layer == 10)
        {
            transform.position = restartPosition;
        }
    }
}
