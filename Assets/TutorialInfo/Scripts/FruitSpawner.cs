using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;
    public float spawnInterval = 3f;
    public float platformRadius = 15f;
    public float spawnHeight = 2f;
    public int maxFruits = 10;
    
    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime && GetCurrentFruitCount() < maxFruits)
        {
            SpawnFruit();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnFruit()
    {
        // Get random angle
        float angle = Random.Range(0f, 360f);
        
        // Convert to position on circle edge
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * platformRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * platformRadius;
        
        // Create spawn position
        Vector3 spawnPos = new Vector3(x, spawnHeight, z);
        
        // Spawn fruit facing center
        GameObject fruit = Instantiate(fruitPrefab, spawnPos, Quaternion.identity);
        fruit.transform.LookAt(new Vector3(0, spawnHeight, 0));

        // Set up directional arrow for the new fruit
        GameObject arrow = GameObject.Find("DirectionalArrow");
        if (arrow != null)
        {
            arrow.GetComponent<DirectionalArrow>().SetTarget(fruit.transform);
        }
    }

    int GetCurrentFruitCount()
    {
        return Object.FindObjectsByType<FruitEnemy>(FindObjectsSortMode.None).Length;
    }
}