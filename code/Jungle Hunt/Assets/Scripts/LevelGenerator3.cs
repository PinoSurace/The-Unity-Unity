using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelGenerator3 : MonoBehaviour {

    public Transform smallBoulderPrefab;
    public Transform bigBoulderPrefab;

    public Transform backgroundPrefab;
    public Transform groundPrefab;

    private const float boulderSpawnIntervalMin = 3.0f; // As a function of difficulty?
    private const float boulderSpawnIntervalMax = 5.0f; // As a function of difficulty?
    private float nextBoulderSpawnTime = 0.0f;

    private const float levelDuration = 60.0f; // As a function of difficulty?
    private const float firstSpawnTimeOffset = 3.0f;
    private const float runningSpeed = 2.0f;

    void Start()
    {
        nextBoulderSpawnTime = Time.time + firstSpawnTimeOffset + Random.Range(boulderSpawnIntervalMin, boulderSpawnIntervalMax);
    }
    
    void Update()
    {
        SpawnBoulders();
    }

    /**
     * @brief Spawns small or big boulders at random time intervals the left side of the screen
     */
    private void SpawnBoulders()
    {
        if (Time.time >= nextBoulderSpawnTime)
        {
            Vector2 boulderSpawnPosition = new Vector2(-11.0f, 0.5f);

            // Probability that the spawned boulder is small
            const float probabilitySmall = 0.8f;
            
            // It must be between 0 and 1
            Assert.IsTrue(probabilitySmall >= 0.0f && probabilitySmall <= 1.0f);

            bool boulderIsSmall = (Random.Range(0.0f, 1.0f) <= probabilitySmall ? true : false);

            if (boulderIsSmall)
            {
                var boulder = Instantiate(smallBoulderPrefab, boulderSpawnPosition, Quaternion.identity);
            }
            else
            {
                var boulder = Instantiate(bigBoulderPrefab, boulderSpawnPosition, Quaternion.identity);
            }

            nextBoulderSpawnTime = Time.time + Random.Range(boulderSpawnIntervalMin, boulderSpawnIntervalMax);
        }
    }
}
