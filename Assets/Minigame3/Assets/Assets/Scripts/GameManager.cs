using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    enum GameStage { Input, Color }

    // ================= UI =================
    public TMP_Text displayText;

    public TMP_Text instructionText;
    public TMP_Text feedbackText;
    public TMP_InputField xInput;
    public TMP_InputField yInput;

    public TMP_Text colorStageInstructionText;
    public TMP_Text colorStageFeedbackText;
    public TMP_Text alphabetValueText;
    public TMP_Text colorTableText;

    public TMP_Text timerText;

    public Image colorDisplay;

    public GameObject inputStageUI;
    public GameObject colorStageUI;

    // ================= GAME STATE =================
    public Difficulty currentDifficulty = Difficulty.Easy;
    GameStage currentStage;

    int correctX;
    int correctY;
    int Z;
    Color targetColor;

    // ================= TIMER =================
    float stageTimer;
    bool timerRunning;

    // ================= COLORS =================
    Color[] colors =
    {
        Color.white,
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue
    };

    int colorIndex = 0;

    // ================= SYMBOL TABLES =================
    Dictionary<char, int> easyLetters = new Dictionary<char, int>();
    Dictionary<char, int> mediumSymbols = new Dictionary<char, int>()
    {
        {'+',5}, {'-',7}, {'*',9}, {'/',11}, {'%',13}
    };
    Dictionary<char, int> hardSymbols = new Dictionary<char, int>()
    {
        {'@',10}, {'#',15}, {'$',20}, {'&',25}, {'!',30}
    };

    void Start()
    {
        SetupEasyLetters();
        SetupColorTable();
        GeneratePuzzle();
    }

    void Update()
    {
        if (!timerRunning) return;

        stageTimer -= Time.deltaTime;
        timerText.text = "TIME: " + Mathf.CeilToInt(stageTimer);

        if (stageTimer <= 0)
        {
            timerRunning = false;
            StageFailed();
        }
    }

    // ================= PUZZLE =================
    void GeneratePuzzle()
    {
        feedbackText.text = "";
        colorStageFeedbackText.text = "";
        xInput.text = "";
        yInput.text = "";

        int number;
        char s1, s2;
        int v1, v2;

        if (currentDifficulty == Difficulty.Easy)
        {
            instructionText.text =
                "STEP 1: Find X\n→ Add the digits\n\n" +
                "STEP 2: Find Y\n→ Add alphabet values";

            number = Random.Range(10, 100);
            s1 = (char)Random.Range('A', 'Z' + 1);
            s2 = (char)Random.Range('A', 'Z' + 1);

            v1 = easyLetters[s1];
            v2 = easyLetters[s2];

            correctX = (number / 10) + (number % 10);
            correctY = v1 + v2;
        }
        else if (currentDifficulty == Difficulty.Medium)
        {
            instructionText.text =
                "STEP 1: Find X\n→ Add all digits\n\n" +
                "STEP 2: Find Y\n→ Add symbol values";

            number = Random.Range(100, 1000);
            s1 = GetRandomKey(mediumSymbols);
            s2 = GetRandomKey(mediumSymbols);

            v1 = mediumSymbols[s1];
            v2 = mediumSymbols[s2];

            correctX = (number / 100) + ((number / 10) % 10) + (number % 10);
            correctY = v1 + v2;
        }
        else
        {
            instructionText.text =
                "STEP 1: Find X\n→ Add digits then double\n\n" +
                "STEP 2: Find Y\n→ Add symbols then double";

            number = Random.Range(100, 1000);
            s1 = GetRandomKey(hardSymbols);
            s2 = GetRandomKey(hardSymbols);

            v1 = hardSymbols[s1];
            v2 = hardSymbols[s2];

            correctX = ((number / 100) + ((number / 10) % 10) + (number % 10)) * 2;
            correctY = (v1 + v2) * 2;
        }

        displayText.text = number + " " + s1 + s2;
        alphabetValueText.text = s1 + " = " + v1 + " , " + s2 + " = " + v2;

        Z = correctX * correctY;
        targetColor = GetColorFromZ(Z);

        colorIndex = 0;
        colorDisplay.color = colors[colorIndex];

        SetStage(GameStage.Input);
    }

    // ================= STAGES =================
    void SetStage(GameStage stage)
    {
        currentStage = stage;

        if (stage == GameStage.Input)
        {
            inputStageUI.SetActive(true);
            colorStageUI.SetActive(false);

            StartTimer(GetInputTime());
        }
        else
        {
            inputStageUI.SetActive(false);
            colorStageUI.SetActive(true);

            colorStageInstructionText.text =
                "STEP 3: Z = X x Y\nUse the color table\nConfirm correct color";

            StartTimer(GetColorTime());
        }
    }

    // ================= TIMER CONTROL =================
    void StartTimer(float time)
    {
        stageTimer = time;
        timerRunning = true;
        timerText.gameObject.SetActive(true);
    }

    void StopTimer()
    {
        timerRunning = false;
    }

    float GetInputTime()
    {
        if (currentDifficulty == Difficulty.Easy) return 30f;
        if (currentDifficulty == Difficulty.Medium) return 25f;
        return 20f;
    }

    float GetColorTime()
    {
        if (currentDifficulty == Difficulty.Easy) return 20f;
        if (currentDifficulty == Difficulty.Medium) return 15f;
        return 10f;
    }

    void StageFailed()
    {
        timerText.text = "TIME UP!";
        feedbackText.text = "Time expired!";
        colorStageFeedbackText.text = "Time expired!";
        Invoke(nameof(GeneratePuzzle), 2f);
    }

    // ================= INPUT CHECK =================
    public void VerifyInputs()
    {
        int userX, userY;

        if (!int.TryParse(xInput.text, out userX) ||
            !int.TryParse(yInput.text, out userY))
        {
            feedbackText.text = "Enter valid numbers!";
            return;
        }

        if (userX == correctX && userY == correctY)
        {
            StopTimer();
            feedbackText.text = "Correct!";
            SetStage(GameStage.Color);
        }
        else
        {
            feedbackText.text = "Wrong! Try again.";
        }
    }

    // ================= COLOR =================
    public void CycleColor()
    {
        colorIndex = (colorIndex + 1) % colors.Length;
        colorDisplay.color = colors[colorIndex];
    }

    public void ConfirmColor()
    {
        StopTimer();

        if (colors[colorIndex] == targetColor)
        {
            colorStageFeedbackText.text = " SUCCESS!";
            AdvanceDifficulty();
        }
        else
        {
            colorStageFeedbackText.text = " FAILED!";
            Invoke(nameof(GeneratePuzzle), 2f);
        }
    }

    // ================= DIFFICULTY =================
    void AdvanceDifficulty()
    {
        if (currentDifficulty == Difficulty.Easy)
            currentDifficulty = Difficulty.Medium;
        else if (currentDifficulty == Difficulty.Medium)
            currentDifficulty = Difficulty.Hard;
        else
            colorStageFeedbackText.text = " ALL LEVELS CLEARED!";

        Invoke(nameof(GeneratePuzzle), 2f);
    }

    // ================= HELPERS =================
    Color GetColorFromZ(int z)
    {
        if (z <= 59) return Color.white;
        if (z <= 99) return Color.red;
        if (z <= 199) return Color.yellow;
        if (z <= 299) return Color.green;
        if (z <= 399) return Color.blue;
        if (z <= 499) return Color.yellow;
        if (z <= 599) return Color.red;
        return Color.white;
    }

    char GetRandomKey(Dictionary<char, int> dict)
    {
        List<char> keys = new List<char>(dict.Keys);
        return keys[Random.Range(0, keys.Count)];
    }

    void SetupEasyLetters()
    {
        for (char c = 'A'; c <= 'Z'; c++)
            easyLetters[c] = c - 'A' + 1;
    }

    void SetupColorTable()
    {
        colorTableText.text =
            "Z COLOR TABLE\n" +
            "0-59 WHITE\n60-99 RED\n100-199 YELLOW\n" +
            "200-299 GREEN\n300-399 BLUE\n" +
            "400-499 YELLOW\n500-599 RED\n600+ WHITE";
    }
}
