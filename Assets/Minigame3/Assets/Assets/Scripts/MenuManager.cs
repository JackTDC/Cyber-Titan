using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject gameUI; // Parent object for gameplay UI

    bool isPaused = false;

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameUI.activeSelf)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ================= MAIN MENU =================
    public void PlayGame()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
    }

    // ================= PAUSE MENU =================
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);   // 👈 THIS IS THE FIX
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);    // 👈 SHOW GAME AGAIN
        Time.timeScale = 1f;
        isPaused = false;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
