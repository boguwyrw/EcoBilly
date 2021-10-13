using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBilly : MonoBehaviour
{
    [SerializeField] ParticleSystem splash;

    float[] stagesSpeed = new float[3];
    float billyWalkingSpeed = 0.0f;
    //int maxDamages = 5;
    int finishedStages = 0;
    bool isRestarted = false;

    Animator billyAnimator;
    Vector3 velocity;
    Vector3 startPosition;
    Vector3 restartPosition;
    PlayerBillyRotation playerBillyRotation;

    void Start()
    {
        stagesSpeed[0] = 6.5f;
        stagesSpeed[1] = 7.5f;
        stagesSpeed[2] = 8.5f;
        billyWalkingSpeed = stagesSpeed[finishedStages];

        billyAnimator = GetComponent<Animator>();

        startPosition = transform.position;
        restartPosition = startPosition;

        playerBillyRotation = GetComponent<PlayerBillyRotation>();
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

        SplashDelay();
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

    void SplashDelay()
    {
        if (splash.isPlaying)
        {
            GameController.Instance.startGame = false;
        }
        else if (!splash.isPlaying && isRestarted)
        {
            isRestarted = false;
            GameController.Instance.startGame = true;
            billyWalkingSpeed = stagesSpeed[finishedStages];
        }
    }

    void BackToCheckpointPosition()
    {
        isRestarted = true;
        billyWalkingSpeed = 0.0f;
        transform.position = restartPosition;
        splash.Play();
    }

    void SetFinishRotation()
    {
        if (transform.localEulerAngles.y < 360.0f && transform.localEulerAngles.y > (360.0f - playerBillyRotation.maxAngleY))
        {
            playerBillyRotation.rotationLeft = true;
        }
        else if (transform.localEulerAngles.y > 0.0f && transform.localEulerAngles.y < playerBillyRotation.maxAngleY)
        {
            playerBillyRotation.rotationRight = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            GameController.Instance.damages += 1;
            if (GameController.Instance.damages == GameController.Instance.maxDamages)
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
        if (other.gameObject.layer == 8)
        {
            billyWalkingSpeed = 0.0f;
            GameController.Instance.finishedGame = true;
            SetFinishRotation();
        }

        if (other.gameObject.layer == 9)
        {
            BoxCollider otherBoxCollider = other.gameObject.GetComponent<BoxCollider>();
            float positionZ = otherBoxCollider.center.z;
            restartPosition = new Vector3(transform.position.x, transform.position.y, positionZ);
            finishedStages += 1;
            billyWalkingSpeed = stagesSpeed[finishedStages];
            otherBoxCollider.enabled = false;
        }

        if (other.gameObject.layer == 10)
        {
            BackToCheckpointPosition();
            GameController.Instance.BillysLifesSystem();
        }
    }
}
