using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBilly : MonoBehaviour
{
    float billyWalkingSpeed = 5.0f;
    float billyRotationSpeed = 160.0f;
    float billysAngle = 0.0f;
    float maxAngle = 60.0f;
    float minAngle = 1.5f;
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

            SetBillysAngles();

            BillysRotation();
        }

        BillysAnimation();

        ExitGame();
    }

    void BillysMovement()
    {
        velocity = Vector3.forward * billyWalkingSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

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

    void BillysAnimation()
    {
        billyAnimator.SetFloat("Speed", velocity.z);
    }

    void BackToCheckpointPosition()
    {

    }

    void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("Hit");
            BackToCheckpointPosition();
        }

        if (collision.gameObject.layer == 7)
        {
            GameController.Instance.points += 1;
            Destroy(collision.gameObject);
        }
    }
}
