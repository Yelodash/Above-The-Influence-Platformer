using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnTime = 1f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnPrefab();
            timer = 0f;
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned!");
        }
    }
}