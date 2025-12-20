using UnityEngine;
using TMPro;
using UnityEngine.UI;


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

    [Header("Clock Visual")]
    public Image clockFillImage;

    public System.Action OnTimeUp;


    private float blinkTimer;
    private bool isVisible;
    private int lastSecond = -1;


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
        int currentSecond = Mathf.CeilToInt(timeLeft);

        // Update text only when the second changes
        if (currentSecond != lastSecond)
        {
            timerText.text = currentSecond.ToString();
            lastSecond = currentSecond;
        }

        // Warning state (last 10 seconds)
        if (timeLeft <= 10f)
        {
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= blinkSpeed)
            {
                blinkTimer = 0f;
                isVisible = !isVisible;
            }

            timerText.color = warningColor;
            timerText.enabled = isVisible;
        }
        else
        {
            timerText.color = normalColor;
            timerText.enabled = true;
        }

        // Radial clock fill
        if (clockFillImage != null)
            clockFillImage.fillAmount = timeLeft / startTime;
    }


    public void StartTimer()
    {
        timeLeft = startTime;
        timerRunning = true;
        blinkTimer = 0f;
        isVisible = true;

        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();
            lastSecond = Mathf.CeilToInt(timeLeft);

        }

        if (resumeButton != null) resumeButton.SetActive(false);
        if (continueButton != null) continueButton.SetActive(false);

        if (passwordInputPanel != null)
            passwordInputPanel.SetActive(true);


        if (clockFillImage != null)
            clockFillImage.fillAmount = 1f;
    
    }

    void TimeUp()
    {
        timerRunning = false;
        timeLeft = 0;

        timerText.text = "0";
        timerText.color = warningColor;

        if (passwordInputPanel != null)
            passwordInputPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.SetActive(true);

        if (continueButton != null)
            continueButton.SetActive(false);

        if (clockFillImage != null)
            clockFillImage.fillAmount = 0f;

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
