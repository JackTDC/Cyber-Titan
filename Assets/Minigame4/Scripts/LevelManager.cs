using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject accessGrantedPanel;

    [Header("Timer")]
    public CountdownTimer countdownTimer;

    [Header("Level Tracking")]
    public int currentLevelIndex = 0;

    void Start()
    {
        // Ensure panel is hidden at start
        if (accessGrantedPanel != null)
            accessGrantedPanel.SetActive(false);
    }

    // ✅ Called when password/minigame is correct
    public void ShowAccessGranted(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        if (countdownTimer != null)
            countdownTimer.StopTimerOnSuccess();

        if (accessGrantedPanel != null)
        {
            accessGrantedPanel.SetActive(true);
            Debug.Log("Access Granted Panel SHOWN");
        }
        else
        {
            Debug.LogError("AccessGrantedPanel is NOT assigned in LevelManager");
        }
    }

    // ✅ Continue button
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
