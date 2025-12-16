using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime = 1.5f;
    public float rangeX = 6f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnTime);
    }

    void SpawnEnemy()
    {
        Vector3 pos = new Vector3(
            Random.Range(-rangeX, rangeX),
            transform.position.y,
            0
        );

        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}
