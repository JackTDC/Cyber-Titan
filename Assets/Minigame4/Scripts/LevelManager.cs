using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject accessGrantedPanel;
    public Timer countdownTimer; // Reference to your existing Timer class
    public GameObject resumeButton; // Resume button

    [Header("Level Tracking")]
    public int currentLevelIndex = 0; // Track the current level

    // Called when password/minigame is correct
    public void ShowAccessGranted(int levelIndex)
    {
        currentLevelIndex = levelIndex; // Save current level

        if (accessGrantedPanel != null)
            accessGrantedPanel.SetActive(true);

        if (countdownTimer != null)
            countdownTimer.StopTimerOnSuccess();

        if (resumeButton != null)
            resumeButton.SetActive(false);
    }

    // Continue button or Resume button logic
    public void ContinueGame(bool isResume = false)
    {
        if (isResume)
        {
            Debug.Log("Resuming level " + currentLevelIndex);

            // Reset timer
            if (countdownTimer != null)
                countdownTimer.ResetTimer();

            // Hide panels
            if (accessGrantedPanel != null)
                accessGrantedPanel.SetActive(false);
            if (resumeButton != null)
                resumeButton.SetActive(false);

            // Restart minigame at current level
            FinalGuessGameManager fgm = Object.FindFirstObjectByType<FinalGuessGameManager>();
            if (fgm != null)
                fgm.RestartFromLevel(currentLevelIndex);
        }
        else
        {
            // Continue to next scene
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextSceneIndex);
            else
                Debug.Log("No more scenes. Game complete!");
        }
    }

    // Resume button onClick
    public void OnResumeButtonClicked()
    {
        ContinueGame(true);
    }
}
