// Nathan Stoffel
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab; // Assign your zombie asset

    public Transform[] spawnPoints; // Array of potential spawn points
    public Transform player; // Reference to the player's Transform
    public Camera mainCamera; // Reference to the main camera
    public static int currentWave = 1;
    public int baseAmount = 5; // Base number of zombies in the first wave
    public int increment = 3; // Zombies added per wave
    public float timeBetweenSpawn = 4f;
    private int zombiesAlive = 0; // Tracks zombies alive
    private float elapsedTime = 0f;
    public static float timePassed;
    private Dictionary<Transform, bool> spawnPointVisibility;


    public UserInterface userInterface;

    void Start()
    {
        spawnPointVisibility = new Dictionary<Transform, bool>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            spawnPointVisibility[spawnPoint] = false; // Initialize all spawn points as not visible
        }
        StartCoroutine(SpawnWave());

        
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timePassed = elapsedTime;

        // Update visibility of spawn points
        UpdateSpawnPointVisibility();

        // Start the next wave when all zombies are destroyed
        if (zombiesAlive == 0)
        {
            currentWave++;
            StartCoroutine(SpawnWave());
        }

        if (userInterface != null)
        {
            userInterface.UpdateWaveDisplay(currentWave);
        }

    }

    void UpdateSpawnPointVisibility()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            spawnPointVisibility[spawnPoint] = IsSpawnPointVisibleToCamera(spawnPoint);
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
                Transform spawnPoint = GetRandomSpawnPointWithinDistance(75f);

                if (spawnPoint != null && !spawnPointVisibility[spawnPoint])
                {
                    Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
                    spawned = true;
                }
                else
                {
                    Debug.LogWarning("No valid spawn points found or spawn point is visible. Retrying...");
                    yield return new WaitForSeconds(0.5f); // Wait before retrying
                }
            }

            yield return new WaitForSeconds(timeBetweenSpawn); // Delay between spawning zombies
        }
    }

    Transform GetRandomSpawnPointWithinDistance(float maxDistance)
    {
        Transform[] validPoints = System.Array.FindAll(spawnPoints, point =>
            Vector3.Distance(player.position, point.position) <= maxDistance);

        if (validPoints.Length > 0)
        {
            return validPoints[Random.Range(0, validPoints.Length)];
        }

        return null;
    }

    bool IsSpawnPointVisibleToCamera(Transform spawnPoint)
    {
        // Get the viewport position of the spawn point
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(spawnPoint.position);

        // Check if the point is within the camera's viewport
        bool isVisibleOnScreen = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                                 viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                                 viewportPoint.z > 0; // Check if in front of the camera

        if (isVisibleOnScreen)
        {
            return true; // Spawn point is visible directly on the screen
        }

        // If not visible on the screen, perform a raycast to check for obstructions
        RaycastHit hit;
        Vector3 direction = spawnPoint.position - mainCamera.transform.position;

        // Perform the raycast
        if (Physics.Raycast(mainCamera.transform.position, direction, out hit))
        {
            // Check if the ray hit the spawn point directly
            return hit.transform == spawnPoint;
        }

        return false; // Not visible if outside the viewport and raycast doesn't directly hit the spawn point
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
