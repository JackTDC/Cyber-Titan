using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    public float startTime = 30f;
    private float timeLeft;

    public TextMeshProUGUI timerText;

    [Header("UI References")]
    public GameObject gameplayPanel;
    public GameObject resumeButton;

    [Header("Game Manager")]
    public FinalGuessGameManager gameManager;

    private bool isRunning = true;
    private bool isBlinking = false;

    void Start()
    {
        ResetTimer();

        if (resumeButton != null)
            resumeButton.SetActive(false);
    }

    void Update()
    {
        if (!isRunning || timeLeft <= 0) return;

        timeLeft -= Time.deltaTime;

        // 🔴 Start blinking + red at last 10s
        if (timeLeft <= 10f && !isBlinking)
        {
            StartCoroutine(BlinkRed());
        }

        if (timeLeft <= 0)
        {
            TimeUp();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timeLeft > 0)
            timerText.text = "Time left: " + Mathf.Ceil(timeLeft);
        else
            timerText.text = "TIME'S UP";
    }

    void TimeUp()
    {
        isRunning = false;
        timeLeft = 0;

        // Hide gameplay
        if (gameplayPanel != null)
            gameplayPanel.SetActive(false);

        // Show only Resume button
        if (resumeButton != null)
            resumeButton.SetActive(true);

        timerText.color = Color.red;
        UpdateUI();
    }

    IEnumerator BlinkRed()
    {
        isBlinking = true;
        Color original = timerText.color;

        while (timeLeft > 0 && timeLeft <= 10f)
        {
            timerText.color = Color.red;
            yield return new WaitForSeconds(0.4f);

            timerText.color = original;
            yield return new WaitForSeconds(0.4f);
        }

        timerText.color = original;
        isBlinking = false;
    }

    // 🔁 Called when game / level starts or continues
    public void ResetTimer()
    {
        StopAllCoroutines();

        timeLeft = startTime;
        isRunning = true;
        isBlinking = false;

        timerText.color = Color.white;

        if (resumeButton != null)
            resumeButton.SetActive(false);

        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);

        UpdateUI();
    }

    // ▶ Resume button calls this
    public void ResumeGame()
    {
        ResetTimer(); // restart timer
        if (gameManager != null)
            gameManager.RestartFromLevel1();
    }

    // ❌ Call this from Enter button click
    public void StopTimer()
    {
        isRunning = false;
        StopAllCoroutines();
    }
}
