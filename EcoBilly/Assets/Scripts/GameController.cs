using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [SerializeField] Transform playerBilly;
    [SerializeField] Text pointsText;

    float sideLimit = 5.0f;
    float straightLineLimit = 0.50f;

    [HideInInspector] public Vector3 clickPosition;
    [HideInInspector] public int direction = 0;
    [HideInInspector] public int points = 0;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool canTurnLeft = false;
    [HideInInspector] public bool canTurnRight = false;

    public Image startImage;

    void Update()
    {
        if (Input.touchCount == 1 && startGame)
        {
            Touch touch = Input.GetTouch(0);
            clickPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                SetBillysDirection();
            }
            
            pointsText.text = points.ToString();
        }
        else if (!startGame)
        {
            direction = 0;
        }
    }

    void SetBillysDirection()
    {
        if ((clickPosition.x > Screen.width / 2) && playerBilly.position.x < sideLimit)
        {
            direction = 1;
        }

        if ((clickPosition.x < Screen.width / 2) && playerBilly.position.x > -sideLimit)
        {
            direction = -1;
        }
    }

    public bool IsPointerOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        return results.Count > 0;
    }
}
