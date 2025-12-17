using UnityEngine;

public class PasswordSpawner : MonoBehaviour
{
    public GameObject passwordPrefab; // Your PasswordCard prefab
    public RectTransform canvasRect;  // Assign your Canvas here

    [System.Serializable]
    public class PasswordData
    {
        public string text;
        public bool isCorrect;
    }

    public PasswordData[] passwords; // List of all passwords (correct + wrong)

    public float spawnDelay = 1.2f; // Time between spawns

    void Start()
    {
        InvokeRepeating(nameof(SpawnPassword), 1f, spawnDelay);
    }

    void SpawnPassword()
    {
        // Spawn new card
        GameObject obj = Instantiate(passwordPrefab, transform);
        RectTransform rt = obj.GetComponent<RectTransform>();

        // Random X position within canvas
        float randomX = Random.Range(
            -canvasRect.rect.width / 2 + 50,
             canvasRect.rect.width / 2 - 50
        );

        // Start above the screen
        rt.anchoredPosition = new Vector2(randomX, canvasRect.rect.height / 2 + 50);

        // Assign random password
        PasswordCard card = obj.GetComponent<PasswordCard>();
        PasswordData data = passwords[Random.Range(0, passwords.Length)];
        card.SetPassword(data.text, data.isCorrect);
    }
}
