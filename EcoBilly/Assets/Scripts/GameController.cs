using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    [SerializeField] GameObject buttonsGO;
    [SerializeField] GameObject congratulationsPanelGO;
    [SerializeField] GameObject gameOverPanelGO;
    [SerializeField] Text pointsText;
    [SerializeField] Text damagesText;
    [SerializeField] Sprite eliminatedBilly;
    [SerializeField] ParticleSystem fireworks;

    Image[] billysLifes;
    int wastedLives = 0;
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

        LoseGame();

        WinFireworksExplosion();

        ExitGame();
    }

    public void BillysLifesSystem()
    {
        billysLifes[wastedLives].sprite = eliminatedBilly;
        damages = 0;
        wastedLives += 1;
    }

    void LoseGame()
    {
        if (wastedLives == 3)
        {
            startGame = false;
            endGame = true;
            playerBilly.gameObject.SetActive(false);
            buttonsGO.SetActive(true);
            gameOverPanelGO.SetActive(true);
        }
    }

    void WinFireworksExplosion()
    {
        if (finishedGame && !fireworksFired)
        {
            fireworks.Play();
            fireworksFired = true;
            buttonsGO.SetActive(true);
            congratulationsPanelGO.SetActive(true);
        }
    }

    void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveGame()
    {
        Application.Quit();
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
