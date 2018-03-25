using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Level generator script for level 2. The script generates bubbles
 * and crocodiles at random time intervals and positions. Also the
 * background and the level ending trigger are generated.
 */
public class LevelGenerator2 : MonoBehaviour {
    public Transform bubblePrefab;
    public Transform crocodilePrefab;
    public Transform levelEndPrefab;

    public Transform skyBackgroundPrefab;
    public Transform riverBackgroundPrefab;

    private int difficulty;

    private float levelDuration;
    private float levelStartTime;

    private float firstSpawnTimeOffset;
    private float lastSpawnTimeOffset;
    private float swimmingSpeed;

    private float bubbleSpawnIntervalMin;
    private float bubbleSpawnIntervalMax;
    private float nextBubbleSpawnTime;

    private float crocodileSpawnIntervalMin;
    private float crocodileSpawnIntervalMax;

    void Start()
    {
        try
        {
            difficulty = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>().GetDifficulty();
        }
        catch
        {
            difficulty = 2;
        }

        levelStartTime = Time.time;

        levelDuration = 60.0f;

        firstSpawnTimeOffset = 3.0f;
        lastSpawnTimeOffset = 5.0f;
        swimmingSpeed = 2.0f;

        bubbleSpawnIntervalMin = 1.0f;
        bubbleSpawnIntervalMax = 7.0f / difficulty;
        nextBubbleSpawnTime = 0.0f;

        crocodileSpawnIntervalMin = 1.0f;
        crocodileSpawnIntervalMax = 5.0f / difficulty;

        nextBubbleSpawnTime = levelStartTime + firstSpawnTimeOffset
            + Random.Range(bubbleSpawnIntervalMin, bubbleSpawnIntervalMax);

        GenerateBackground();
        SpawnCrocodiles();
        SpawnLevelEnd();
    }
    
    void Update()
    {
        if (Time.time < levelStartTime + levelDuration - lastSpawnTimeOffset &&
            Time.time >= nextBubbleSpawnTime)
        {
            SpawnBubble();
        }
    }

    /**
     * @brief Spawns bubbles at random time intervals and in random positions at the bottom of the screen
     */
    private void SpawnBubble()
    {
        const float bubbleSpawnXMin = -10.0f;
        const float bubbleSpawnXMax = 1.0f;
        float bubbleSpawnX = Random.Range(bubbleSpawnXMin, bubbleSpawnXMax);

        var bubble = Instantiate(bubblePrefab, new Vector3(bubbleSpawnX, -5.0f, 0.0f), Quaternion.identity);
        var bubbleSpeedY = bubble.GetComponent<Rigidbody2D>().velocity.y;
        bubble.GetComponent<Rigidbody2D>().velocity = new Vector2(swimmingSpeed, bubbleSpeedY);

        nextBubbleSpawnTime = Time.time + Random.Range(bubbleSpawnIntervalMin, bubbleSpawnIntervalMax);
    }

    /**
     * @brief Spawns crocodiles at random positions in the level
     */
    private void SpawnCrocodiles()
    {
        // This needs to be swimmingSpeed + something, so that it seems the crocodiles are
        // also swimming against you and not being stationary or even swimming backwards
        float crocodileSpeed = swimmingSpeed + 2.0f + (float)difficulty;

        // This is the level length in terms of where to spawn crocodiles. Some will be spawned
        // further than the level ending, but that's because they will then reach the player position
        // at the right time before the level ends.
        float levelTotalLength = crocodileSpeed  * (levelDuration + firstSpawnTimeOffset);

        for (float crocodileSpawnX = -crocodileSpeed * firstSpawnTimeOffset - Random.Range(crocodileSpawnIntervalMin, crocodileSpawnIntervalMax);
             crocodileSpawnX > -levelTotalLength + crocodileSpeed * (crocodileSpawnIntervalMax + lastSpawnTimeOffset);
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

        var levelEnd = Instantiate(levelEndPrefab, new Vector3(-levelTotalLength, -2.0f, 0.0f), Quaternion.identity);
        levelEnd.gameObject.name = "NextLevelCollider";
        levelEnd.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(swimmingSpeed, 0.0f);
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
