using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab; // Assign your zombie asset

    public Transform[] spawnPoints; // Array of potential spawn points
    public Transform player; // Reference to the player's Transform
    public Camera mainCamera; // Reference to the main camera
    public int currentWave = 1;
    public int baseAmount = 5; // Base number of zombies in the first wave
    public int increment = 3; // Zombies added per wave
    private int zombiesAlive = 0; // Tracks zombies alive
    private float elapsedTime = 0f;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Start the next wave when all zombies are destroyed
        if (zombiesAlive == 0)
        {
            currentWave++;
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        int numberOfZombies = baseAmount + (currentWave - 1) * increment;
        zombiesAlive = numberOfZombies; // Set zombies alive to match the wave size

        for (int i = 0; i < numberOfZombies; i++)
        {
            bool spawned = false;

            while (!spawned)
            {
                // Get a valid spawn point within 150 meters of the player
                Transform spawnPoint = GetRandomSpawnPointWithinDistance(150f);

                if (spawnPoint != null && !IsSpawnPointVisibleToCamera(spawnPoint))
                {
                    Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
                    spawned = true;
                }
                else
                {
                    Debug.LogWarning("No valid spawn points found or spawn point not visible. Retrying...");
                    yield return new WaitForSeconds(2f); // Wait 2 seconds before retrying
                }
            }

            yield return new WaitForSeconds(2f); // Delay between spawning zombies
        }
    }

    Transform GetRandomSpawnPointWithinDistance(float maxDistance)
    {
        // Filter spawn points to only those within the specified distance of the player
        Transform[] validPoints = System.Array.FindAll(spawnPoints, point =>
            Vector3.Distance(player.position, point.position) <= maxDistance);

        // If there are valid points, pick one at random
        if (validPoints.Length > 0)
        {
            return validPoints[Random.Range(0, validPoints.Length)];
        }

        return null; // No valid spawn points found
    }

    bool IsSpawnPointVisibleToCamera(Transform spawnPoint)
    {
        // Raycast from the camera to the spawn point
        RaycastHit hit;
        Vector3 direction = spawnPoint.position - mainCamera.transform.position;

        // Perform the raycast to check for any obstructions
        if (Physics.Raycast(mainCamera.transform.position, direction, out hit))
        {
            // If the hit object is not the spawn point, it's blocked
            if (hit.transform != spawnPoint)
            {
                return false;
            }
        }

        return true; // If the ray hits the spawn point directly, it's visible
    }

    public void ZombieDestroyed()
    {
        zombiesAlive--; // Reduce the count when a zombie is destroyed
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
