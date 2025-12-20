using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 30f;
    private float timeLeft;
    private bool timerRunning;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public GameObject passwordInputPanel;
    public GameObject resumeButton;
    public GameObject continueButton;

    [Header("Timer Visuals")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float blinkSpeed = 0.5f;

    private float blinkTimer;
    private bool isVisible;

    void Awake()
    {
        timerRunning = false;
        isVisible = true;
        blinkTimer = 0f;

        if (resumeButton != null) resumeButton.SetActive(false);
        if (continueButton != null) continueButton.SetActive(false);
    }

    void Update()
    {
        if (!timerRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft > 0)
        {
            UpdateTimerUI();
        }
        else
        {
            TimeUp();
        }
    }

    void UpdateTimerUI()
    {
        if (timeLeft <= 10f)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkSpeed)
            {
                blinkTimer = 0f;
                isVisible = !isVisible;
            }

            timerText.color = warningColor;
            timerText.text = isVisible
                ? "Time Left: " + Mathf.Ceil(timeLeft) + "s"
                : "";
        }
        else
        {
            timerText.color = normalColor;
            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft) + "s";
        }
    }

    public void StartTimer()
    {
        timeLeft = startTime;
        timerRunning = true;
        blinkTimer = 0f;
        isVisible = true;

        if (timerText != null)
        {
            timerText.color = normalColor;
            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft) + "s";
        }

        if (resumeButton != null) resumeButton.SetActive(false);
        if (continueButton != null) continueButton.SetActive(false);

        if (passwordInputPanel != null)
            passwordInputPanel.SetActive(true);
    }

    void TimeUp()
    {
        timerRunning = false;
        timeLeft = 0;

        timerText.text = "TIME'S UP!";
        timerText.color = warningColor;

        if (passwordInputPanel != null)
            passwordInputPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.SetActive(true);

        if (continueButton != null)
            continueButton.SetActive(false);
    }

    public void StopTimerOnSuccess()
    {
        timerRunning = false;

        if (timerText != null)
            timerText.text = "";

        if (resumeButton != null)
            resumeButton.SetActive(false);

        if (continueButton != null)
            continueButton.SetActive(true);
    }
}
