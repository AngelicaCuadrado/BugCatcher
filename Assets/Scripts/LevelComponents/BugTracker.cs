using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Use TextMeshProUGUI for nice UI text

public class BugTracker : MonoBehaviour
{
    [Header("Bug Requirements")]
    public int butterflyRequirement = 5;
    public int ladybugRequirment = 5;

    [Header("Current Counts (read-only at runtime)")]
    public int currentButterflies;
    public int currentLadybugs;

    [Header("Scoring")]
    public int scorePerButterfly = 10;
    public int scorePerLadybug = 15;
    public int winBonus = 100;
    public int CurrentScore { get; private set; }

    [Header("Timer")]
    [Tooltip("Total time for the level in seconds")]
    public float levelTime = 60f;

    private float remainingTime;
    private bool levelEnded = false;

    [Header("UI (Optional)")]
    public TMP_Text scoreText;
    public TMP_Text timerText;

    private void Start()
    {
        // Reset counters
        currentButterflies = 0;
        currentLadybugs = 0;
        CurrentScore = 0;

        remainingTime = levelTime;
        levelEnded = false;

        UpdateScoreUI();
        UpdateTimerUI();

        Debug.Log($"[BugTracker] Level started. Time: {levelTime} seconds");
    }

    private void Update()
    {
        if (levelEnded) return;

        // --- TIMER ---
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            UpdateTimerUI();
            TimeUpLose();   // Lose when time runs out
            return;
        }

        UpdateTimerUI();
    }

    // Called by Net when catching a butterfly
    public void RegisterButterflyCaught()
    {
        if (levelEnded) return;

        currentButterflies++;
        CurrentScore += scorePerButterfly;

        Debug.Log($"[BugTracker] Butterflies: {currentButterflies}/{butterflyRequirement}");
        UpdateScoreUI();
        CheckWinCondition();
    }

    // Called by Net when catching a ladybug
    public void RegisterLadybugCaught()
    {
        if (levelEnded) return;

        currentLadybugs++;
        CurrentScore += scorePerLadybug;

        Debug.Log($"[BugTracker] Ladybugs: {currentLadybugs}/{ladybugRequirment}");
        UpdateScoreUI();
        CheckWinCondition();
    }

    // ---------- WIN / LOSE LOGIC ----------

    private void CheckWinCondition()
    {
        // Win when required # of each bug is collected
        if (currentButterflies >= butterflyRequirement &&
            currentLadybugs >= ladybugRequirment)
        {
            LevelWon();
        }
    }

    private void LevelWon()
    {
        if (levelEnded) return;
        levelEnded = true;

        CurrentScore += winBonus;
        UpdateScoreUI();

        Debug.Log("[BugTracker] WIN! All required bugs caught.");
        SceneManager.LoadScene("WinScreen");   // <--- Name of your Win scene
    }

    private void TimeUpLose()
    {
        if (levelEnded) return;
        levelEnded = true;

        Debug.Log("[BugTracker] LOSE! Time is up.");
        SceneManager.LoadScene("LoseScreen");  // <--- Name of your Lose scene
    }

    // ---------- UI HELPERS ----------

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{CurrentScore}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(remainingTime);
            timerText.text = $"{seconds}";
        }
    }
}
