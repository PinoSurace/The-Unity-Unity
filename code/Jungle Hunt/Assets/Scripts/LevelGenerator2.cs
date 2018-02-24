using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Level generator script for level 2. The script generates bubbles
 * and crocodiles at random time intervals in random positions.
 */
public class LevelGenerator2 : MonoBehaviour {

    public Transform bubblePrefab;
    public Transform crocodilePrefab;

    private bool levelEndSpawned = false;
    private const float levelDuration = 30.0f; // As a function of difficulty?
    private const float firstSpawnTimeOffset = 3.0f;

    private const float bubbleSpawnIntervalMin = 2.0f; // As a function of difficulty?
    private const float bubbleSpawnIntervalMax = 10.0f; // As a function of difficulty?
    private float nextBubbleSpawnTime = 0.0f;

    private const float crocodileSpawnIntervalMin = 2.0f; // As a function of difficulty?
    private const float crocodileSpawnIntervalMax = 10.0f; // As a function of difficulty?
    private float nextCrocodileSpawnTime = 0.0f;

    void Start()
    {
        nextBubbleSpawnTime = Time.time + firstSpawnTimeOffset
            + Random.Range(bubbleSpawnIntervalMin, bubbleSpawnIntervalMax);
        nextCrocodileSpawnTime = Time.time + firstSpawnTimeOffset
            + Random.Range(crocodileSpawnIntervalMin, crocodileSpawnIntervalMax);
    }
    
    void Update()
    {
        if (Time.time < levelDuration)
        {
            SpawnBubbles();
            SpawnCrocodiles();
        }
        else if (!levelEndSpawned)
        {
            SpawnLevelEnd();
        }
    }

    /**
     * @brief Spawns bubbles at random time intervals and in
     *        random positions at the bottom of the screen
     */
    void SpawnBubbles()
    {
        if (Time.time >= nextBubbleSpawnTime)
        {
            const float bubbleSpawnXMin = -5.0f;
            const float bubbleSpawnXMax = 0.0f;
            float bubbleSpawnX = Random.Range(bubbleSpawnXMin, bubbleSpawnXMax);

            var bubble = Instantiate(bubblePrefab, new Vector3(bubbleSpawnX, -5.0f, 0.0f), Quaternion.identity);
            var bubbleSpeedY = bubble.GetComponent<Rigidbody2D>().velocity.y;
            bubble.GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, bubbleSpeedY);

            nextBubbleSpawnTime = Time.time + Random.Range(bubbleSpawnIntervalMin, bubbleSpawnIntervalMax);
        }
    }

    /**
     * @brief Spawns crocodiles at random time intervals and in
     *        random positions at the left side of the screen
     */
    void SpawnCrocodiles()
    {
        if (Time.time >= nextCrocodileSpawnTime)
        {
            const float crocodileSpeed = 2.0f; // As a function of difficulty?
            const float crocodileSpawnYMin = -3.0f;
            const float crocodileSpawnYMax = -1.0f;
            float crocodileSpawnY = Random.Range(crocodileSpawnYMin, crocodileSpawnYMax);

            var crocodile = Instantiate(crocodilePrefab, new Vector3(-7.0f, crocodileSpawnY, 0.0f), Quaternion.identity);
            crocodile.GetComponent<Rigidbody2D>().velocity = new Vector2(crocodileSpeed, 0.0f);

            nextCrocodileSpawnTime = Time.time + Random.Range(crocodileSpawnIntervalMin, crocodileSpawnIntervalMax);
        }
    }

    /**
     * @brief Spawns the level ending collider and graphics
     */
    void SpawnLevelEnd()
    {
        levelEndSpawned = true;
    }
}
