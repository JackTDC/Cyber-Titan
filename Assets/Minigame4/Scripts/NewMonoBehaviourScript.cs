using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeLeft = 30f; // Total time
    public TextMeshProUGUI timerText; // Timer UI
    public GameObject passwordInputPanel; // Panel containing InputField
    public Color normalColor = Color.white; // Default timer color
    public Color warningColor = Color.red;   // Color for last 10 seconds
    public float blinkSpeed = 1f; // How fast it blinks (seconds)

    private bool timerRunning = true;
    private float blinkTimer = 0f;
    private bool isVisible = true;

    void Update()
    {
        if (!timerRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft > 0)
        {
            // Last 10 seconds: change color and blink
            if (timeLeft <= 10f)
            {
                // Handle blinking
                blinkTimer += Time.deltaTime;
                if (blinkTimer >= blinkSpeed)
                {
                    blinkTimer = 0f;
                    isVisible = !isVisible;
                }

                timerText.color = warningColor;
                timerText.text = isVisible ? "Time Left: " + Mathf.Ceil(timeLeft).ToString() + "s" : "";
            }
            else
            {
                // Normal timer
                timerText.color = normalColor;
                timerText.text = "Time Left: " + Mathf.Ceil(timeLeft).ToString() + "s";
            }
        }
        else
        {
            // Time's up
            timerText.text = "Time's Up!";
            timerText.color = warningColor;
            timerRunning = false;
            LockScreen();
        }
    }

    void LockScreen()
    {
        passwordInputPanel.SetActive(false);
    }
}
