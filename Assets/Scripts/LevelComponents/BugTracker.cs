using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BugTracker : MonoBehaviour
{
    public static BugTracker Instance { get; private set; }

    [Header("Targets")]
    [SerializeField] private int requiredLadybugs = 3;
    [SerializeField] private int requiredButterflies = 5;
    [Tooltip("If true, counts will be clamped to the required values (won't exceed).")]
    [SerializeField] private bool clampToRequired = false;

    [Header("Scoring")]
    [SerializeField] private int scorePerButterfly = 10;
    [SerializeField] private int scorePerLadybug = 15;
    [SerializeField] private int winBonus = 100;
    public int CurrentScore { get; private set; }
    [SerializeField] public int highScore = 0;

    [Header("Timer")]
    [Tooltip("Total time for the level in seconds")]
    [SerializeField] private float levelTime = 60f;
    private float remainingTime;
    private bool levelEnded = false;

    [Header("UI (Optional)")]
    [SerializeField] private TMP_Text ladybugText;
    [SerializeField] private TMP_Text butterflyText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Scene Names")]
    [SerializeField] private string winSceneName = "WinScreen";
    [SerializeField] private string loseSceneName = "LoseScreen";

    private int currentLadybugs;
    private int currentButterflies;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentLadybugs = 0;
        currentButterflies = 0;
        CurrentScore = 0;

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        remainingTime = levelTime;
        levelEnded = false;

        UpdateUI();
        UpdateTimerUI();

        Debug.Log($"[BugTracker] Level started. Time: {levelTime} seconds");
    }

    private void Update()
    {
        if (levelEnded) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            UpdateTimerUI();
            TimeUpLose();
            return;
        }

        UpdateTimerUI();
    }

    public void RegisterButterflyCaught()
    {
        AddButterfly(1);
    }

    public void RegisterLadybugCaught()
    {
        AddLadybug(1);
    }

    public void AddLadybug(int amount = 1)
    {
        if (levelEnded) return;

        int newCount = currentLadybugs + amount;
        if (clampToRequired)
            currentLadybugs = Mathf.Clamp(newCount, 0, requiredLadybugs);
        else
            currentLadybugs = Mathf.Max(0, newCount);

        CurrentScore += scorePerLadybug * amount;

        CheckHighScore();
        Debug.Log($"[BugTracker] Ladybugs: {currentLadybugs}/{requiredLadybugs}");
        UpdateUI();
        CheckWinCondition();
    }

    public void AddButterfly(int amount = 1)
    {
        if (levelEnded) return;

        int newCount = currentButterflies + amount;
        if (clampToRequired)
            currentButterflies = Mathf.Clamp(newCount, 0, requiredButterflies);
        else
            currentButterflies = Mathf.Max(0, newCount);

        CurrentScore += scorePerButterfly * amount;

        CheckHighScore();
        Debug.Log($"[BugTracker] Butterflies: {currentButterflies}/{requiredButterflies}");
        UpdateUI();
        CheckWinCondition();
    }

    private void CheckHighScore()
    {
        if (CurrentScore > highScore)
        {
            highScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    private void CheckWinCondition()
    {
        if (currentButterflies >= requiredButterflies &&
            currentLadybugs >= requiredLadybugs)
        {
            LevelWon();
        }
    }

    private void LevelWon()
    {
        if (levelEnded) return;
        levelEnded = true;

        CurrentScore += winBonus;

        CheckHighScore();
        UpdateUI();

        Debug.Log("[BugTracker] WIN! All required bugs caught.");
        if (!string.IsNullOrEmpty(winSceneName))
            SceneManager.LoadScene(winSceneName);
    }

    private void TimeUpLose()
    {
        if (levelEnded) return;
        levelEnded = true;

        Debug.Log("[BugTracker] LOSE! Time is up.");
        if (!string.IsNullOrEmpty(loseSceneName))
            SceneManager.LoadScene(loseSceneName);
    }

    private void UpdateUI()
    {
        if (ladybugText != null)
            ladybugText.text = $"{currentLadybugs}/{requiredLadybugs}";

        if (butterflyText != null)
            butterflyText.text = $"{currentButterflies}/{requiredButterflies}";

        if (scoreText != null)
            scoreText.text = $"{CurrentScore}";

        if (highScoreText != null)
            highScoreText.text = $"{highScore}";
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int totalSeconds = Mathf.CeilToInt(Mathf.Max(0f, remainingTime));
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public int CurrentLadybugs => currentLadybugs;
    public int CurrentButterflies => currentButterflies;
    public int RequiredLadybugs => requiredLadybugs;
    public int RequiredButterflies => requiredButterflies;
}