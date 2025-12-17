using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public int maxScore = 10;

    public TextMeshProUGUI scoreText;

    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score;

        if (score >= maxScore)
        {
            Time.timeScale = 0;
            Debug.Log("YOU WIN!");
        }
    }
}
