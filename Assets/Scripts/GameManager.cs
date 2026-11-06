using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum Objective
{
    CollectCoins,   // collect targetScore coins
    ReachGoal,      // reach the Goal (usually against a timer)
    Survive,        // stay alive until the timer runs out
    CoinsThenGoal   // collect targetScore coins, THEN the Goal opens
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level rules (change these per level)")]
    [SerializeField] string levelName = "Level 1";
    [SerializeField] Objective objective = Objective.CollectCoins;
    [SerializeField] string objectiveText = "Collect all the coins";
    [SerializeField] int targetScore = 5;
    [SerializeField] float timeLimit = 0f;          // 0 = no timer
    [SerializeField] GameObject goal;               // only for ReachGoal / CoinsThenGoal
    [SerializeField] int lives = 3;
    [SerializeField] float respawnDelay = 0.6f, spawnProtection = 1.5f;

    [Header("Scene references")]
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] TMP_Text levelText, scoreText, livesText, timerText;
    [SerializeField] GameObject winPanel, losePanel;
    [SerializeField] TMP_Text winTitle, nextButtonLabel, loseTitle;

    int score;
    float timeLeft;
    bool ended;
    string currentObjective;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;                 // timeScale survives a scene load
        timeLeft = timeLimit;
        currentObjective = objectiveText;
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
       
        if (goal) goal.SetActive(objective == Objective.ReachGoal);
    }

    void Start() => UpdateUI();

    void Update()
    {
        if (ended || timeLimit <= 0f) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            if (objective == Objective.Survive) CompleteLevel();
            else Lose("Time's up!");
        }
        UpdateTimerText();
    }

    public void AddScore(int amount)
    {
        if (ended) return;
        score += amount;
        UpdateUI();

        if (score < targetScore) return;
        if (objective == Objective.CollectCoins)
        {
            CompleteLevel();
        }
        else if (objective == Objective.CoinsThenGoal && goal && !goal.activeSelf)
        {
            goal.SetActive(true);
            currentObjective = "Coins done! Reach the green exit!";
            UpdateUI();
        }
    }

    public void ReachGoal()
    {
        if (ended) return;
        if (objective == Objective.ReachGoal ||
            (objective == Objective.CoinsThenGoal && score >= targetScore))
            CompleteLevel();
    }

   
    public void UpdateUI()
    {
        if (levelText) levelText.text = levelName + ": " + currentObjective;

        bool coinGoal = objective == Objective.CollectCoins || objective == Objective.CoinsThenGoal;
        if (scoreText) scoreText.text = coinGoal ? $"Coins: {score} / {targetScore}" : $"Coins: {score}";

        if (livesText && player)
            livesText.text = $"Lives: {lives}   Health: {Mathf.Max(player.Current, 0)} / {player.Max}";

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        if (!timerText) return;
        if (timeLimit <= 0f) { timerText.text = ""; return; }
        int secs = Mathf.CeilToInt(timeLeft);
        timerText.text = objective == Objective.Survive ? $"Survive: {secs}s" : $"Time: {secs}s";
    }

   
    public void PlayerDied()
    {
        if (ended) return;
        lives--;
        UpdateUI();
        if (lives <= 0) Lose("Out of lives!");
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        Rigidbody body = player.GetComponent<Rigidbody>();
        body.position = respawnPoint.position;
        body.linearVelocity = Vector3.zero;     
        player.Restore(spawnProtection);
    }

   
    void CompleteLevel()
    {
        if (ended) return;
        ended = true;

        bool last = SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings;
        if (winTitle) winTitle.text = last ? "YOU BEAT ALL 5 LEVELS!" : levelName + " complete!";
        if (nextButtonLabel) nextButtonLabel.text = last ? "Play Again" : "Next Level";

        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose(string reason)
    {
        if (ended) return;
        ended = true;
        if (loseTitle) loseTitle.text = reason;
        if (losePanel) losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    
    public void NextLevel()
    {
        Time.timeScale = 1f;
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next >= SceneManager.sceneCountInBuildSettings) next = 0;   
        SceneManager.LoadScene(next);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
