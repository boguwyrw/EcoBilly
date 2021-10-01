using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBilly : MonoBehaviour
{
    float billyWalkingSpeed = 5.2f;
    float billyRotationSpeed = 150.0f;
    float billysAngle = 0.0f;
    float maxAngle = 70.0f;
    float minAngle = 1.5f;
    float sideLimit = 2.8f;
    bool turnedAround = false;
    Animator billyAnimator;
    Vector3 velocity;
    // granica: 4.2f i -4.2f

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

            BoundariesDetection();
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
        if (transform.localEulerAngles.y < 360 && transform.localEulerAngles.y >= 250)
        {
            billysAngle = 360 - transform.localEulerAngles.y;
        }
        else if (transform.localEulerAngles.y > 0 && transform.localEulerAngles.y <= 110)
        {
            billysAngle = transform.localEulerAngles.y;
        }
    }

    void BillysRotation()
    {
        /*
        if (billysAngle > maxAngle && GameController.Instance.direction == -1)
        {
            GameController.Instance.direction = 1;
            turnedAround = true;
            //Debug.Log("Turn left");
        }
        else if (billysAngle > maxAngle && GameController.Instance.direction == 1)
        {
            GameController.Instance.direction = -1;
            turnedAround = true;
            //Debug.Log("Turn right");
        }
        
        if (billysAngle < 1.5f && turnedAround)
        {
            if (GameController.Instance.direction == 1 && transform.position.x < -1.5f)
            {
                //Debug.Log("Turn left side");
                transform.position = new Vector3(-sideLimit, transform.position.y, transform.position.z);
            }
            else if (GameController.Instance.direction == -1 && transform.position.x > 1.5f)
            {
                //Debug.Log("Turn right side");
                transform.position = new Vector3(sideLimit, transform.position.y, transform.position.z);
            }
            else if ((GameController.Instance.direction == 1 && transform.position.x > -0.5f) || (GameController.Instance.direction == -1 && transform.position.x < 0.5f))
            {
                //Debug.Log("Middle");
                transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
            }

            GameController.Instance.direction = 0;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0.0f, transform.localEulerAngles.z);
            billysAngle = 0.0f;
            turnedAround = false;
        }
        */

        if ((GameController.Instance.direction == 1 && billysAngle > minAngle) || (GameController.Instance.direction == -1 && billysAngle > minAngle))
        {
            turnedAround = true;
        }

        SetStraightPosition();

        if (billysAngle > maxAngle && turnedAround)
        {
            GameController.Instance.direction = 0;
            if (transform.localEulerAngles.y <= 290.0f && transform.localEulerAngles.y >= 90.0f)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 290.0f, transform.localEulerAngles.z);
            }
            else if (transform.localEulerAngles.y >= 70.0f)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 70.0f, transform.localEulerAngles.z);
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

    void BoundariesDetection()
    {
        if (transform.position.x > sideLimit && billysAngle > maxAngle)
        {
            GameController.Instance.direction = -1;
        }

        if (transform.position.x < -sideLimit && billysAngle > maxAngle)
        {
            GameController.Instance.direction = 1;
        }
    }

    

    void BillysAnimation()
    {
        billyAnimator.SetFloat("Speed", velocity.z);
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

        }
    }
}
