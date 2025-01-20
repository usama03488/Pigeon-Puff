using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform playerTransform;
    public float spawnInterval = 3f; // Time interval between spawns
    public float spawnDistance = 10f; // Distance above the player to spawn obstacles

    private float timer = 0f;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        // Initialize the last player position
        lastPlayerPosition = playerTransform.position;
    }

    void Update()
    {
        // Calculate the player's movement direction
        Vector3 playerMovement = playerTransform.position - lastPlayerPosition;

        // Check if the player is moving upwards
        if (playerMovement.y > 0)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Check if it's time to spawn a new obstacle
            if (timer >= spawnInterval)
            {
                // Reset the timer
                timer = 0f;

                // Calculate the position to spawn the obstacle
                Vector3 spawnPosition = new Vector3(
                    playerTransform.position.x + Random.Range(-5f, 5f), // Randomize X position near the player
                    playerTransform.position.y + spawnDistance, // Spawn above the player by spawnDistance
                    playerTransform.position.z // Maintain same Z position as the player
                );

                // Spawn the obstacle at the calculated position
                Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            }
        }

        // Update the last player position
        lastPlayerPosition = playerTransform.position;
    }
}
