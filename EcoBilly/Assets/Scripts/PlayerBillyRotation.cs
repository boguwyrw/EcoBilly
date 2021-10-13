using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBillyRotation : MonoBehaviour
{
    [SerializeField] Joystick joystick = null;

    float direction = 0.0f;
    float swipeRange = 100.0f;
    float rotationSpeed = 120.0f;
    Vector2 touchStartPosition;
    Vector2 touchCurrentPosition;

    Touch touch;

    [HideInInspector] public float maxAngleY = 75.0f;
    [HideInInspector] public bool rotationLeft = false;
    [HideInInspector] public bool rotationRight = false;

    void Update()
    {
        if (Input.touchCount > 0 && GameController.Instance.startGame && !GameController.Instance.endGame)
        {
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                SetRotatePosition();
            }
        }
    }

    void FixedUpdate()
    {
        if (GameController.Instance.startGame && !GameController.Instance.finishedGame)
        {
            //transform.localEulerAngles = new Vector3(0.0f, maxAngleY * joystick.Horizontal, 0.0f); // joystick rotation
            transform.localEulerAngles = new Vector3(0.0f, maxAngleY * direction, 0.0f); // swipe finger rotation
        }
        else if (!GameController.Instance.startGame && !GameController.Instance.finishedGame)
        {
            direction = 0;
            transform.localEulerAngles = new Vector3(0.0f, maxAngleY * direction, 0.0f);
        }
        else if (GameController.Instance.finishedGame)
        {
            FinalRotation();
        }
    }

    void SetRotatePosition()
    {
        touchCurrentPosition = touch.position;
        Vector2 currentDistance = touchCurrentPosition - touchStartPosition;

        if ((touchCurrentPosition.x > touchStartPosition.x) && (currentDistance.x < swipeRange))
        {
            if (direction < 1.0f)
                direction = currentDistance.x / swipeRange;
            else
                direction = 1;
        }
        else if ((touchCurrentPosition.x < touchStartPosition.x) && (currentDistance.x < swipeRange))
        {
            if (direction > -1.0f)
                direction = currentDistance.x / swipeRange;
            else
                direction = -1;
        }
    }

    void FinalRotation()
    {
        if (rotationLeft)
        {
            transform.Rotate(0.0f, -rotationSpeed * Time.deltaTime, 0.0f);
            if (transform.localEulerAngles.y < 180.0f)
            {
                FinalPosition();
                rotationLeft = false;
            }
        }
        else if (rotationRight)
        {
            transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
            if (transform.localEulerAngles.y > 180.0f)
            {
                FinalPosition();
                rotationRight = false;
            }
        }
    }

    void FinalPosition()
    {
        transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
    }
}
