using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    static bool skipMenu = false;

    public TMP_Text livesText, missesText, coinsText, gameOverText;
    public GameObject restartButton;
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public int coinsPerFood = 10;
    public int maxLives = 3;
    public int maxMisses = 5;

    int coins, lives, misses;
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        lives = maxLives;
        gameOverPanel.SetActive(false);
        UpdateUI();

        if (skipMenu)
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        skipMenu = false;
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CatchFood()
    {
        if (IsGameOver) return;
        coins += coinsPerFood;
        UpdateUI();
    }

    public void CatchBad()
    {
        if (IsGameOver) return;
        lives--;
        UpdateUI();
        if (lives <= 0) EndGame();
    }

    public void CatchBomb()
    {
        if (IsGameOver) return;
        lives = 0;
        UpdateUI();
        EndGame();
    }

    public void MissFood()
    {
        if (IsGameOver) return;
        misses++;
        UpdateUI();
        if (misses >= maxMisses) EndGame();
    }

    void UpdateUI()
    {
        livesText.text = lives.ToString();
        missesText.text = misses + "/" + maxMisses;
        coinsText.text = coins.ToString();
    }

    void EndGame()
    {
        IsGameOver = true;
        gameOverText.text = "GAME OVER\n" + coins;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        skipMenu = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        skipMenu = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}