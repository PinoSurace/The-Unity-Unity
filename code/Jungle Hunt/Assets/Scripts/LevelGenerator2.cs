using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Level generator script for level 2. The script generates bubbles
 * and crocodiles at random time intervals and positions.
 */
public class LevelGenerator2 : MonoBehaviour {

    public Transform bubblePrefab;
    public Transform crocodilePrefab;

    public Transform skyBackgroundPrefab;
    public Transform riverBackgroundPrefab;
    
    private const float levelDuration = 60.0f; // As a function of difficulty?
    private const float firstSpawnTimeOffset = 3.0f;
    private const float swimmingSpeed = 2.0f;

    private const float bubbleSpawnIntervalMin = 1.0f; // As a function of difficulty?
    private const float bubbleSpawnIntervalMax = 6.0f; // As a function of difficulty?
    private float nextBubbleSpawnTime = 0.0f;

    private const float crocodileSpawnIntervalMin = 1.0f; // As a function of difficulty?
    private const float crocodileSpawnIntervalMax = 4.0f; // As a function of difficulty?

    void Start()
    {
        nextBubbleSpawnTime = Time.time + firstSpawnTimeOffset
            + Random.Range(bubbleSpawnIntervalMin, bubbleSpawnIntervalMax);

        GenerateBackground();
        SpawnCrocodiles();
        SpawnLevelEnd();
    }
    
    void Update()
    {
        if (Time.time < levelDuration)
        {
            SpawnBubbles();
        }
    }

    /**
     * @brief Spawns bubbles at random time intervals and in random positions at the bottom of the screen
     */
    private void SpawnBubbles()
    {
        if (Time.time >= nextBubbleSpawnTime)
        {
            const float bubbleSpawnXMin = -10.0f;
            const float bubbleSpawnXMax = -5.0f;
            float bubbleSpawnX = Random.Range(bubbleSpawnXMin, bubbleSpawnXMax);

            var bubble = Instantiate(bubblePrefab, new Vector3(bubbleSpawnX, -5.0f, 0.0f), Quaternion.identity);
            var bubbleSpeedY = bubble.GetComponent<Rigidbody2D>().velocity.y;
            bubble.GetComponent<Rigidbody2D>().velocity = new Vector2(swimmingSpeed, bubbleSpeedY);

            nextBubbleSpawnTime = Time.time + Random.Range(bubbleSpawnIntervalMin, bubbleSpawnIntervalMax);
        }
    }

    /**
     * @brief Spawns crocodiles at random positions in the level
     */
    private void SpawnCrocodiles()
    {
        // This needs to be swimmingSpeed + something, so that it seems the crocodiles are
        // also swimming against you and not being stationary or even swimming backwards
        const float crocodileSpeed = swimmingSpeed + 2.0f; // As a function of difficulty?

        // This is the level length in terms of where to spawn crocodiles. Some will be spawned
        // further than the level ending, but that's because they will then reach the player position
        // at the right time before the level ends.
        float levelTotalLength = crocodileSpeed  * (levelDuration + firstSpawnTimeOffset);

        for (float crocodileSpawnX = -crocodileSpeed * firstSpawnTimeOffset - Random.Range(crocodileSpawnIntervalMin, crocodileSpawnIntervalMax);
             crocodileSpawnX > -levelTotalLength;
             crocodileSpawnX -= crocodileSpeed * Random.Range(crocodileSpawnIntervalMin, crocodileSpawnIntervalMax))
        {
            const float crocodileSpawnYMin = -3.0f;
            const float crocodileSpawnYMax = 1.5f;
            float crocodileSpawnY = Random.Range(crocodileSpawnYMin, crocodileSpawnYMax);

            var crocodile = Instantiate(crocodilePrefab, new Vector3(crocodileSpawnX, crocodileSpawnY, 0.0f), Quaternion.identity);
            crocodile.GetComponent<Rigidbody2D>().velocity = new Vector2(crocodileSpeed, 0.0f);
        }
    }

    /**
     * @brief Spawns the level ending collider and graphics
     */
    private void SpawnLevelEnd()
    {
        float levelTotalLength = swimmingSpeed * (levelDuration + firstSpawnTimeOffset);

        GameObject gameObject = new GameObject("NextLevelCollider");

        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(3.0f, 10.0f);
        collider.isTrigger = true;
        collider.transform.position = new Vector2(-levelTotalLength, 0.0f);

        Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.velocity = new Vector2(swimmingSpeed, 0.0f);
    }

    /**
     * @brief Generates the background images for the whole duration of the level
     */
    private void GenerateBackground()
    {
        float skyBackgroundImageWidth = skyBackgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float riverBackgroundImageWidth = riverBackgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        float backgroundTotalLength = swimmingSpeed * (levelDuration + firstSpawnTimeOffset);

        // Add some extra just to be sure there's enough background at the end of the level
        backgroundTotalLength += (skyBackgroundImageWidth > riverBackgroundImageWidth
                                  ? 2*skyBackgroundImageWidth
                                  : 2*riverBackgroundImageWidth);

        for (float skyBackgroundX = 0.0f, riverBackgroundX = 0.0f;
            (skyBackgroundX > -backgroundTotalLength) && (riverBackgroundX > -backgroundTotalLength);
            skyBackgroundX -= skyBackgroundImageWidth, riverBackgroundX -= riverBackgroundImageWidth)
        {
            var skyBackground = Instantiate(skyBackgroundPrefab,
                                            new Vector3(skyBackgroundX, 0.0f, 100.0f),
                                            Quaternion.identity);
            skyBackground.GetComponent<Rigidbody2D>().velocity = new Vector2(swimmingSpeed, 0.0f);

            var riverBackground = Instantiate(riverBackgroundPrefab,
                                              new Vector3(riverBackgroundX, -2.0f, 99.0f),
                                              Quaternion.identity);
            riverBackground.GetComponent<Rigidbody2D>().velocity = new Vector2(swimmingSpeed, 0.0f);
        }
    }
}
