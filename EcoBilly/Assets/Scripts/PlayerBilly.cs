using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBilly : MonoBehaviour
{
    [SerializeField] ParticleSystem splash;
    [SerializeField] ParticleSystem rakeRangePS;
    [SerializeField] ParticleSystem cloudPS;

    float[] stagesSpeed = new float[3];
    float billyWalkingSpeed = 0.0f;
    float rakeTime = 0.0f;
    float wateringTime = 0.0f;
    float rakeTimeValue = 6.5f;
    float wateringTimeValue = 7.5f;
    int finishedStages = 0;
    bool isRestarted = false;
    bool canActiveRake = false;
    bool canActiveWatering = false;

    Animator billyAnimator;
    Vector3 velocity;
    Vector3 startPosition;
    Vector3 restartPosition;
    PlayerBillyRotation playerBillyRotation;
    SphereCollider rakeRangeSC;

    void Start()
    {
        stagesSpeed[0] = 6.5f;
        stagesSpeed[1] = 7.0f;
        stagesSpeed[2] = 8.0f;
        billyWalkingSpeed = stagesSpeed[finishedStages];

        billyAnimator = GetComponent<Animator>();

        startPosition = transform.position;
        restartPosition = startPosition;

        playerBillyRotation = GetComponent<PlayerBillyRotation>();
        rakeRangeSC = GetComponent<SphereCollider>();

        rakeTime = rakeTimeValue;
        wateringTime = wateringTimeValue;
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

        WateringFunctionality();

        AssignLifetimeValue(wateringTime);
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

    void WateringFunctionality()
    {
        if (canActiveWatering)
        {
            wateringTime -= Time.deltaTime;

            if (wateringTime <= 0.0f)
            {
                TurnOffWatering();
            }
        }
    }

    void TurnOffRakeRange()
    {
        GameController.Instance.BerakingInfoOff();
        rakeRangeSC.enabled = false;
        rakeRangePS.Stop();
        canActiveRake = false;
        rakeTime = rakeTimeValue;
    }

    void TurnOffWatering()
    {
        //AssignLifetimeValue(0.1f);
        GameController.Instance.WateringInfoOff();
        cloudPS.Stop();
        canActiveWatering = false;
        wateringTime = wateringTimeValue;
    }

    void AssignLifetimeValue(float lifetimeValue)
    {
        var cloudMain = cloudPS.main;
        cloudMain.startLifetime = lifetimeValue;
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
        if (collision.gameObject.layer == 6 && !canActiveWatering)
        {
            GameController.Instance.damages += 1;
            GameController.Instance.TouchSound();
            if (GameController.Instance.damages == GameController.Instance.maxDamages)
            {
                BackToCheckpointPosition();
            }
        }
        else if (collision.gameObject.layer == 6 && canActiveWatering)
        {
            GameController.Instance.WateringSound();
            if (GameController.Instance.damages > 0)
            {
                GameController.Instance.damages -= 1;
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

        //if (other.gameObject.layer == 8)
        if (other.gameObject.layer == 8 && !canActiveRake)
        {
            billyWalkingSpeed = 0.0f;
            GameController.Instance.finishedGame = true;
            SetFinishRotation();
        }

        if (other.gameObject.layer == 9)
        {
            BoxCollider otherBoxCollider = other.gameObject.GetComponent<BoxCollider>();
            float positionZ = otherBoxCollider.center.z;
            otherBoxCollider.enabled = false;
            restartPosition = new Vector3(transform.position.x, transform.position.y, positionZ);
            if (finishedStages < 2)
            {
                finishedStages += 1;
            }
            billyWalkingSpeed = stagesSpeed[finishedStages];
        }

        if (other.gameObject.layer == 10)
        {
            BackToCheckpointPosition();
            TurnOffRakeRange();
            TurnOffWatering();
            GameController.Instance.OutOfBorderSound();
            GameController.Instance.BillysLifesSystem();
        }

        if (other.gameObject.layer == 11)
        {
            GameController.Instance.RakeCollectingSound();
            GameController.Instance.BerakingInfoOn();
            rakeRangeSC.enabled = true;
            Destroy(other.transform.parent.gameObject);
            rakeRangePS.Play();
            canActiveRake = true;
        }

        if (other.gameObject.layer == 12)
        {
            GameController.Instance.WateringCanCollectingSound();
            GameController.Instance.WateringInfoOn();
            canActiveWatering = true;
            //AssignLifetimeValue(5.0f);
            cloudPS.Play();
            Destroy(other.transform.parent.gameObject);
        }
    }
}
