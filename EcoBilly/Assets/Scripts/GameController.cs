using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static GameController _instance;

    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    [SerializeField] Text pointsText;

    int points = 0;

    [HideInInspector] public Vector3 clickPosition;
    [HideInInspector] public int direction = 0;

    public Image startImage;
    public bool startGame = false;

    void Update()
    {
        if (startGame)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                clickPosition = touch.position;

                SetBillysDirection();
                /*
                if (touch.phase == TouchPhase.Began)
                {
                    points++;
                }
                */
                pointsText.text = points.ToString();
            }
        }
    }

    void SetBillysDirection()
    {
        if (clickPosition.x > Screen.width / 2)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }
}
