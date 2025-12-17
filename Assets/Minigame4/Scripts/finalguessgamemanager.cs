using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class FinalGuessGameManager : MonoBehaviour
{
    [Header("UI (Existing)")]
    public TMP_InputField passwordInput;
    public Button enterButton;
    public TextMeshProUGUI consoleText;

    [Header("Gameplay Parent")]
    public GameObject gameplayPanel;

    [Header("Access Granted UI")]
    public GameObject AccessGrantedPanel;
    public Button continueButton;

    [Header("Timer")]
    public Timer timer;

    int currentLevel = 0;
    int currentRuleIndex = 0;

    List<Rule> currentRules;

    void Start()
    {
        enterButton.onClick.AddListener(OnEnterPressed);
        LoadLevel(0);

        if (AccessGrantedPanel != null)
            AccessGrantedPanel.SetActive(false);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinuePressed);
    }

    void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;
        currentRuleIndex = 0;

        currentRules = BuildRulesForLevel(levelIndex);

        passwordInput.text = "";
        passwordInput.interactable = true;
        enterButton.interactable = true;
        passwordInput.ActivateInputField();

        if (timer != null)
            timer.ResetTimer();

        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);

        RenderRules();
    }

    void OnEnterPressed()
    {
        string input = passwordInput.text;

        if (string.IsNullOrEmpty(input))
        {
            ShowWarning("Password cannot be empty");
            return;
        }

        Rule rule = currentRules[currentRuleIndex];

        if (!rule.validator(input))
        {
            ShowWarning("Rule failed");
            return;
        }

        currentRuleIndex++;

        if (currentRuleIndex >= currentRules.Count)
        {
            LevelComplete();
            return;
        }

        RenderRules();
    }

    void LevelComplete()
    {
        passwordInput.interactable = false;
        enterButton.interactable = false;

        if (gameplayPanel != null)
            gameplayPanel.SetActive(false);

        if (AccessGrantedPanel != null)
            AccessGrantedPanel.SetActive(true);
    }

    void OnContinuePressed()
    {
        if (AccessGrantedPanel != null)
            AccessGrantedPanel.SetActive(false);

        LoadLevel(currentLevel + 1);
    }

    // 🔁 CALLED BY TIMER WHEN RESUME BUTTON IS CLICKED
    public void RestartFromLevel1()
    {
        if (AccessGrantedPanel != null)
            AccessGrantedPanel.SetActive(false);

        LoadLevel(0); // Level 1
    }

    void ShowWarning(string message)
    {
        consoleText.text += $"\n⚠ {message}";
    }

    void RenderRules()
    {
        consoleText.text = $"LEVEL {currentLevel + 1}\n\n";

        for (int i = 0; i < currentRules.Count; i++)
        {
            string status =
                i < currentRuleIndex ? "[OK]" :
                i == currentRuleIndex ? "[>]" :
                " [] ";

            consoleText.text += $"{status} {currentRules[i].description}\n";
        }
    }

    // ---------- RULE DEFINITIONS ----------
    List<Rule> BuildRulesForLevel(int level)
    {
        var rules = new List<Rule>();

        if (level == 0)
        {
            rules.Add(new Rule("At least 12 characters", s => s.Length >= 12));
            rules.Add(new Rule("Contains uppercase letter", HasUppercase));
            rules.Add(new Rule("Contains a number", HasNumber));
        }
        else if (level == 1)
        {
            rules.Add(new Rule("Exactly 10 characters", s => s.Length == 10));
            rules.Add(new Rule("Exactly one special character", s => CountSpecials(s) == 1));
            rules.Add(new Rule("Starts and ends with a number",
                s => s.Length >= 2 && char.IsDigit(s[0]) && char.IsDigit(s[s.Length - 1])));
        }
        else if (level == 2)
        {
            rules.Add(new Rule("Exactly 8 characters", s => s.Length == 8));
            rules.Add(new Rule("At least 3 numbers", s => CountNumbers(s) >= 3));
            rules.Add(new Rule("Contains uppercase letter", HasUppercase));
            rules.Add(new Rule("No special characters", s => CountSpecials(s) == 0));
        }

        return rules;
    }

    // ---------- HELPERS ----------
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

// ---------- RULE CLASS ----------
public class Rule
{
    public string description;
    public Func<string, bool> validator;

    public Rule(string desc, Func<string, bool> val)
    {
        description = desc;
        validator = val;
    }
}
