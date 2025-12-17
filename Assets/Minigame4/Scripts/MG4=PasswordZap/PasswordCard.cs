using UnityEngine;
using TMPro;

public class PasswordCard : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool isCorrect;

    public void SetPassword(string value, bool correct)
    {
        text.text = value;
        isCorrect = correct;
    }

    void OnMouseDown()
    {
        // Only wrong passwords increase score
        if (!isCorrect)
        {
            ScoreManager.Instance.AddScore(1); // Pass 1 point
            Destroy(gameObject);
        }
        else
        {
            // Optional: click correct password → do nothing or game over
            Debug.Log("Correct password, do not cut!");
        }
    }
}
