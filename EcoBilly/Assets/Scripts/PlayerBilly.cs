using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBilly : MonoBehaviour
{
    [SerializeField] ParticleSystem splash;
    [SerializeField] ParticleSystem rakeRangePS;

    float[] stagesSpeed = new float[3];
    float billyWalkingSpeed = 0.0f;
    float rakeTime = 0.0f;
    float rakeTimeValue = 7.5f;
    int finishedStages = 0;
    bool isRestarted = false;
    bool canActiveRake = false;

    Animator billyAnimator;
    Vector3 velocity;
    Vector3 startPosition;
    Vector3 restartPosition;
    PlayerBillyRotation playerBillyRotation;
    SphereCollider rakeRangeSC;

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
        rakeRangeSC = GetComponent<SphereCollider>();

        rakeTime = rakeTimeValue;
    }

    void FixedUpdate()
    {
        if (GameController.Instance.startGame)
        {
            BillysMovement();
        }

        BillysAnimation();

        SplashDelay();

        RakeFunctionality();
    }

    void BillysMovement()
    {
        velocity = Vector3.forward * billyWalkingSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

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

    void RakeFunctionality()
    {
        if (canActiveRake)
        {
            rakeTime -= Time.deltaTime;

            if (rakeTime <= 0.0f)
            {
                TurnOffRakeRange();
            }
        }
    }

    void TurnOffRakeRange()
    {
        rakeRangeSC.enabled = false;
        rakeRangePS.Stop();
        canActiveRake = false;
        rakeTime = rakeTimeValue;
    }

    void BackToCheckpointPosition()
    {
        isRestarted = true;
        billyWalkingSpeed = 0.0f;
        transform.position = restartPosition;
        TurnOffRakeRange();
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
            GameController.Instance.TouchSound();
            if (GameController.Instance.damages == GameController.Instance.maxDamages)
            {
                BackToCheckpointPosition();
            }
        }

        if (collision.gameObject.layer == 7)
        {
            GameController.Instance.points += 1;
            GameController.Instance.CollectingSound();
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 7) || (other.gameObject.layer == 7 && canActiveRake))
        {
            GameController.Instance.points += 1;
            GameController.Instance.CollectingSound();
        }

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
            TurnOffRakeRange();
            GameController.Instance.OutOfBorderSound();
            GameController.Instance.BillysLifesSystem();
        }

        if (other.gameObject.layer == 11)
        {
            GameController.Instance.RakeCollectingSound();
            rakeRangeSC.enabled = true;
            Destroy(other.transform.parent.gameObject);
            rakeRangePS.Play();
            canActiveRake = true;
        }
    }
}
