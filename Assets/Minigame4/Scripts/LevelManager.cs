using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject accessGrantedPanel;

    [Header("Timer")]
    public CountdownTimer countdownTimer; // ✅ NEW TIMER ONLY

    [Header("Level Tracking")]
    public int currentLevelIndex = 0;

    // Called when password/minigame is correct
    public void ShowAccessGranted(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        if (accessGrantedPanel != null)
            accessGrantedPanel.SetActive(true);

        if (countdownTimer != null)
            countdownTimer.StopTimerOnSuccess();
    }

    // Continue to next scene
    public void ContinueGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes. Game complete!");
        }
    }
}
