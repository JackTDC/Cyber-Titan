using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 30f;
    private float timeLeft;

    public TextMeshProUGUI timerText;

    [Header("UI References")]
    public GameObject gameplayPanel;
    public GameObject resumeButton;

    [Header("Game Manager")]
    public FinalGuessGameManager gameManager;

    private bool isRunning = false;
    private bool isBlinking = false;
    private int timerSession = 0; 

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 10f && timeLeft > 0 && !isBlinking)
            StartCoroutine(BlinkRed(timerSession));

        if (timeLeft <= 0)
        {
            TimeUp();
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time left: " + Mathf.Ceil(timeLeft);
    }

    void TimeUp()
    {
        isRunning = false;
        timeLeft = 0;
        StopAllCoroutines();

        if (gameplayPanel != null)
            gameplayPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.SetActive(true);

        if (timerText != null)
        {
            timerText.color = Color.red;
            timerText.text = "TIME'S UP";
        }
    }

    IEnumerator BlinkRed(int sessionId)
    {
        isBlinking = true;
        Color original = timerText.color;

        while (isRunning && sessionId == timerSession && timeLeft > 0 && timeLeft <= 10f)
        {
            timerText.color = Color.red;
            yield return new WaitForSeconds(0.4f);

            timerText.color = original;
            yield return new WaitForSeconds(0.4f);
        }

        if (timerText != null)
            timerText.color = original;

        isBlinking = false;
    }

    public void ResetTimer()
    {
        timerSession++;
        StopAllCoroutines();

        timeLeft = startTime;
        isRunning = true;
        isBlinking = false;

        if (timerText != null)
        {
            timerText.color = Color.white;
            UpdateUI();
        }

        if (resumeButton != null)
            resumeButton.SetActive(false);

        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);
    }

    // 🔹 Updated ResumeGame to restart current level
    public void ResumeGame()
    {
        ResetTimer();

        if (gameManager != null)
            gameManager.RestartFromLevel(gameManager.currentLevel);
    }

    public void StopTimerOnSuccess()
    {
        timerSession++; 
        isRunning = false;
        StopAllCoroutines();

        timeLeft = startTime;

        if (resumeButton != null)
            resumeButton.SetActive(false);

        if (timerText != null)
            timerText.text = "";
    }
}
