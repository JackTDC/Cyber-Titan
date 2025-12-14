using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public enum LevelType
{
    Level1,
    Level2,
    Level3
}

public class GuessGridManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public Button enterButton;
    public TextMeshProUGUI consoleText;
    public Image[] meterSegments;
    public Slider intrusionSlider;

    LevelType currentLevel = LevelType.Level1;

    int revealedRules = 0;
    int wrongAttempts = 0;

    // RULE SETS
    List<System.Func<string, bool>> rules = new();
    List<string> messages = new();

    void Start()
    {
        enterButton.onClick.RemoveAllListeners();
        enterButton.onClick.AddListener(OnEnter);

        LoadLevel(LevelType.Level1);
    }

    // ================= LEVEL SETUP =================

    void LoadLevel(LevelType level)
    {
        currentLevel = level;
        revealedRules = 0;
        wrongAttempts = 0;

        rules.Clear();
        messages.Clear();

        intrusionSlider.value = 0;
        inputField.text = "";
        inputField.interactable = true;
        enterButton.interactable = true;
        inputField.ActivateInputField();

        if (level == LevelType.Level1)
        {
            consoleText.text = "LEVEL 1\nTutorial System Online";

            rules.Add(s => s.Length >= 12);
            messages.Add("Use at least 12 characters.");

            rules.Add(s => HasUppercase(s));
            messages.Add("Add an uppercase letter.");

            rules.Add(s => HasNumber(s));
            messages.Add("Add a number.");
        }
        else if (level == LevelType.Level2)
        {
            consoleText.text = "LEVEL 2\nLogic System Online";

            rules.Add(s => s.Length == 10);
            messages.Add("Password must be exactly 10 characters.");

            rules.Add(s => CountSpecials(s) == 1);
            messages.Add("Use exactly ONE special character.");

            rules.Add(s => char.IsDigit(s[0]) && char.IsDigit(s[^1]));
            messages.Add("Start and end with a number.");
        }
        else
        {
            consoleText.text = "LEVEL 3\nFinal System Online";

            rules.Add(s => s.Length == 8);
            messages.Add("Password must be exactly 8 characters.");

            rules.Add(s => CountNumbers(s) >= 3);
            messages.Add("At least 3 numbers required.");

            rules.Add(s => HasUppercase(s));
            messages.Add("Must contain an uppercase letter.");

            rules.Add(s => CountSpecials(s) == 0);
            messages.Add("No special characters allowed.");
        }

        UpdateMeter();
    }

    // ================= INPUT =================

    void OnEnter()
    {
        if (!inputField.interactable) return;

        string guess = inputField.text.Trim();
        if (string.IsNullOrEmpty(guess)) return;

        CheckGuess(guess);
        inputField.ActivateInputField();
    }


    // ================= GAME LOGIC =================

    void CheckGuess(string guess)
    {
        // Check already revealed rules
        for (int i = 0; i < revealedRules; i++)
        {
            if (!rules[i](guess))
            {
                RegisterFail();
                return;
            }
        }

        // Reveal next rule
        if (revealedRules < rules.Count)
        {
            consoleText.text = messages[revealedRules];
            revealedRules++;
            UpdateMeter();
            return;
        }

        // All rules revealed AND guess passes all → level complete
        CompleteLevel();
    }

    void RegisterFail()
    {
        wrongAttempts++;
        intrusionSlider.value = wrongAttempts;
        consoleText.text = "Incorrect. Try again.";
    }

    void CompleteLevel()
    {
        if (currentLevel == LevelType.Level1)
            LoadLevel(LevelType.Level2);
        else if (currentLevel == LevelType.Level2)
            LoadLevel(LevelType.Level3);
        else
        {
            consoleText.text = "ALL LEVELS COMPLETE\nACCESS GRANTED";
            inputField.interactable = false;
            enterButton.interactable = false;
        }
    }

    // ================= VISUALS =================

    void UpdateMeter()
    {
        for (int i = 0; i < meterSegments.Length; i++)
            meterSegments[i].color = i < revealedRules ? Color.green : Color.gray;
    }

    // ================= HELPERS =================

    bool HasUppercase(string s) =>
        System.Text.RegularExpressions.Regex.IsMatch(s, "[A-Z]");

    bool HasNumber(string s) =>
        System.Text.RegularExpressions.Regex.IsMatch(s, "[0-9]");

    int CountNumbers(string s)
    {
        int c = 0;
        foreach (char ch in s)
            if (char.IsDigit(ch)) c++;
        return c;
    }

    int CountSpecials(string s)
    {
        int c = 0;
        foreach (char ch in s)
            if ("!@#$%^&*".Contains(ch)) c++;
        return c;
    }
}
