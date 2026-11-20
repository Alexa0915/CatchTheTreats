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

    [Header("Звуки")]
    public AudioClip catchSound;
    public AudioClip hitSound;
    public AudioClip bombSound;
    public AudioClip gameOverSound;
    public AudioClip musicClip;
    [Range(0f, 1f)] public float soundVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.4f;

    AudioSource sfx;
    AudioSource music;

    int coins, lives, misses;
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;

        sfx = gameObject.AddComponent<AudioSource>();
        sfx.playOnAwake = false;

        music = gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        music.loop = true;
    }

    void Start()
    {
        lives = maxLives;
        gameOverPanel.SetActive(false);
        UpdateUI();

        if (musicClip != null)
        {
            music.clip = musicClip;
            music.volume = musicVolume;
            music.Play();
        }

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

    void PlaySound(AudioClip clip)
    {
        if (clip != null) sfx.PlayOneShot(clip, soundVolume);
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
        PlaySound(catchSound);
        UpdateUI();
    }

    public void CatchBad()
    {
        if (IsGameOver) return;
        lives--;
        PlaySound(hitSound);
        UpdateUI();
        if (lives <= 0) EndGame();
    }

    public void CatchBomb()
    {
        if (IsGameOver) return;
        lives = 0;
        PlaySound(bombSound);
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
        PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins", 0) + coins);
        PlayerPrefs.Save();
        gameOverText.text = "GAME OVER\n" + coins;
        gameOverPanel.SetActive(true);
        music.Stop();
        PlaySound(gameOverSound);
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