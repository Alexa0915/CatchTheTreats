using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text livesText, missesText, coinsText, gameOverText;
    public GameObject restartButton;
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
        gameOverText.gameObject.SetActive(false);
        restartButton.SetActive(false);
        UpdateUI();
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
        gameOverText.gameObject.SetActive(true);
        restartButton.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}