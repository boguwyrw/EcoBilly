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
    [SerializeField] GameObject bestScorePanelGO;
    [SerializeField] GameObject berakingPanelGO;
    [SerializeField] GameObject wateringPanelGO;
    [SerializeField] Text pointsText;
    [SerializeField] Text damagesText;
    [SerializeField] Text bestScoreText;
    [SerializeField] Sprite eliminatedBilly;
    [SerializeField] ParticleSystem fireworks;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    Image[] billysLifes;
    int wastedLives = 0;
    bool fireworksFired = false;

    [HideInInspector] public int direction = 0;
    [HideInInspector] public int points = 0;
    [HideInInspector] public int bestScore = 0;
    [HideInInspector] public int damages = 0;
    [HideInInspector] public int maxDamages = 5;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool finishedGame = false;

    public Image startImage;
    public GameObject[] flames;

    void Start()
    {
        billysLifes = new Image[billysLifesGO.transform.childCount];

        for (int i = 0; i < billysLifesGO.transform.childCount; i++)
        {
            billysLifes[i] = billysLifesGO.transform.GetChild(i).GetComponent<Image>();
        }

        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
    }

    void Update()
    {
        pointsText.text = "Points: " + points.ToString();
        bestScoreText.text = "Best score: " + bestScore.ToString();
        damagesText.text = "Plants damages: " + damages.ToString();

        if (bestScore < points)
        {
            bestScore = points;
        }

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

    public void CollectingSound()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    public void RakeCollectingSound()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    public void WateringCanCollectingSound()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    public void TouchSound()
    {
        audioSource.clip = audioClips[3];
        audioSource.Play();
    }

    public void WateringSound()
    {
        audioSource.clip = audioClips[4];
        audioSource.Play();
    }

    public void CheckpointSound()
    {
        audioSource.clip = audioClips[5];
        audioSource.Play();
    }

    public void OutOfBorderSound()
    {
        audioSource.clip = audioClips[6];
        audioSource.Play();
    }

    public void BerakingInfoOn()
    {
        berakingPanelGO.SetActive(true);
    }

    public void BerakingInfoOff()
    {
        berakingPanelGO.SetActive(false);
    }

    public void WateringInfoOn()
    {
        wateringPanelGO.SetActive(true);
    }

    public void WateringInfoOff()
    {
        wateringPanelGO.SetActive(false);
    }

    void LoseGame()
    {
        if (wastedLives == 3)
        {
            startGame = false;
            endGame = true;
            playerBilly.gameObject.SetActive(false);
            ActiveEndGamePanels(gameOverPanelGO);
            BerakingInfoOff();
            WateringInfoOff();
        }
    }

    void WinFireworksExplosion()
    {
        if (finishedGame && !fireworksFired)
        {
            fireworks.Play();
            fireworksFired = true;
            ActiveEndGamePanels(congratulationsPanelGO);
            BerakingInfoOff();
            WateringInfoOff();
        }
    }

    void ActiveEndGamePanels(GameObject rightPanel)
    {
        buttonsGO.SetActive(true);
        rightPanel.SetActive(true);
        bestScorePanelGO.SetActive(true);
        PlayerPrefs.SetInt("BestScore", bestScore);
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
