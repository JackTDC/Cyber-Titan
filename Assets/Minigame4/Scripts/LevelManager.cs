using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject accessGrantedPanel;

    // Show Access Granted panel
    public void ShowAccessGranted()
    {
        accessGrantedPanel.SetActive(true);
    }

    // Called when Continue button is clicked
    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
