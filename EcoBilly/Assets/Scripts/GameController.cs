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
    [SerializeField] Text damagesText;

    float sideLimit = 5.0f;
    float straightLineLimit = 0.50f;

    [HideInInspector] public Vector3 clickPosition;
    [HideInInspector] public int direction = 0;
    [HideInInspector] public int points = 0;
    [HideInInspector] public int damages = 0;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool canTurnLeft = false;
    [HideInInspector] public bool canTurnRight = false;

    public Image startImage;

    void Update()
    {
        /*
        if (Input.touchCount == 1 && startGame)
        {
            Touch touch = Input.GetTouch(0);
            clickPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                SetBillysDirection();
            }
            
            //pointsText.text = "Points: " + points.ToString();
            //damagesText.text = "Damages: " + damages.ToString();
        }
        else if (!startGame)
        {
            direction = 0;
        }
        */
        pointsText.text = "Points: " + points.ToString();
        damagesText.text = "Damages: " + damages.ToString();

        ExitGame();
    }

    /*
    void SetBillysDirection()
    {
        //if ((clickPosition.x > Screen.width / 2) && playerBilly.position.x < sideLimit)
        if (clickPosition.x > Screen.width / 2)
        {
            direction = 1;
        }

        //if ((clickPosition.x < Screen.width / 2) && playerBilly.position.x > -sideLimit)
        if (clickPosition.x < Screen.width / 2)
        {
            direction = -1;
        }
    }
    */

    void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
