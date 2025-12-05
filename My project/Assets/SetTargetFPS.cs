using UnityEngine;

public class SetTargetFPS : MonoBehaviour
{
    [Tooltip("Desired frames per second")]
    public int targetFrameRate = 60;

    void Awake()
    {
        // Disable vSync (lets Application.targetFrameRate take effect)
        QualitySettings.vSyncCount = 0;

        // Set target FPS
        Application.targetFrameRate = targetFrameRate;
    }

    // Optional: keep it locked even if quality changes
    void Update()
    {
        if (Application.targetFrameRate != targetFrameRate)
            Application.targetFrameRate = targetFrameRate;
    }
}
