using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBillyJoystick : MonoBehaviour
{
    [SerializeField] Joystick joystick = null;

    float maxAngleY = 75.0f;
    float direction = 0;
    float swipeRange = 100.0f;
    Vector2 touchStartPosition;
    Vector2 touchCurrentPosition;

    Touch touch;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
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
        if (GameController.Instance.startGame)
        {
            //transform.localEulerAngles = new Vector3(0.0f, maxAngleY * joystick.Horizontal, 0.0f);
            transform.localEulerAngles = new Vector3(0.0f, maxAngleY * direction, 0.0f);
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
}
