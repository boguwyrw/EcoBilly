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
    [SerializeField] GameObject billysLifesGO;
    [SerializeField] Text pointsText;
    [SerializeField] Text damagesText;
    [SerializeField] Sprite eliminatedBilly;
    [SerializeField] ParticleSystem fireworks;

    Image[] billysLifes;
    int lifes = 0;
    bool fireworksFired = false;

    [HideInInspector] public int direction = 0;
    [HideInInspector] public int points = 0;
    [HideInInspector] public int damages = 0;
    [HideInInspector] public int maxDamages = 5;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool finishedGame = false;

    public Image startImage;

    void Start()
    {
        billysLifes = new Image[billysLifesGO.transform.childCount];

        for (int i = 0; i < billysLifesGO.transform.childCount; i++)
        {
            billysLifes[i] = billysLifesGO.transform.GetChild(i).GetComponent<Image>();
        }
    }

    void Update()
    {
        pointsText.text = "Points: " + points.ToString();
        damagesText.text = "Damages: " + damages.ToString();

        if (damages == maxDamages)
        {
            BillysLifesSystem();
        }

        if (lifes == 3)
        {
            startGame = false;
            endGame = true;
            playerBilly.gameObject.SetActive(false);
        }

        FireworksExplosion();

        ExitGame();
    }

    public void BillysLifesSystem()
    {
        billysLifes[lifes].sprite = eliminatedBilly;
        damages = 0;
        lifes += 1;
    }

    void FireworksExplosion()
    {
        if (finishedGame && !fireworksFired)
        {
            fireworks.Play();
            fireworksFired = true;
        }
    }

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
