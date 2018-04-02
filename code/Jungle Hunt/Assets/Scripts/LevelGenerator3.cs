using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelGenerator3 : MonoBehaviour {

    public Transform smallBoulderPrefab;
    public Transform bigBoulderPrefab;

    public Transform backgroundPrefab;
    public Transform groundPrefab;

    private const float slopeAngleDegrees = 10.0f;
    private float slopeAngleRadians = slopeAngleDegrees / 360.0f * 2.0f * Mathf.PI;
    private const float groundOffsetY = -4.5f;

    private const float runningSpeed = 2.0f;

    private const float boulderSpawnVariance = 0.5f; // Not actually variance as in maths, but something like that ...
    private const float boulderSpawnIntervalMin = 2.0f; // As a function of difficulty?
    private const float boulderSpawnIntervalMax = 5.0f; // As a function of difficulty?
    private float nextBoulderSpawnTime = 0.0f;

    private float levelStartTime = 0.0f;
    private const float levelDuration = 60.0f; // As a function of difficulty?
    private const float firstSpawnTimeOffset = 3.0f;
    private const float lastSpawnTimeOffset = 7.0f;
    private float levelTotalLength = runningSpeed * (levelDuration + firstSpawnTimeOffset + lastSpawnTimeOffset);

    private Vector3 nextLevelColliderPosition = new Vector3(0, 0, 0);

    void Start()
    {
        levelStartTime = Time.time;
        nextBoulderSpawnTime = Time.time + firstSpawnTimeOffset + Random.Range(boulderSpawnIntervalMin, boulderSpawnIntervalMax);

        SpawnBackground();
        SpawnGround();
        SpawnNextLevelCollider();
    }

    void Update()
    {
        if (Time.time <= levelStartTime + levelDuration &&
            Time.time >= nextBoulderSpawnTime)
        {
            SpawnBoulders();
        }
    }

    /**
     * @brief Spawns small or big boulders at random time intervals the left side of the screen
     */
    private void SpawnBoulders()
    {
        float boulderSpawnOffset = Random.Range(-boulderSpawnVariance, boulderSpawnVariance);
        Vector2 boulderSpawnPosition = new Vector2(11.0f + boulderSpawnOffset, 0.5f);

        // Probability that the spawned boulder is small
        const float probabilitySmall = 0.5f;

        // It must be between 0 and 1
        Assert.IsTrue(probabilitySmall >= 0.0f && probabilitySmall <= 1.0f);

        bool boulderIsSmall = (Random.Range(0.0f, 1.0f) <= probabilitySmall ? true : false);

        Transform boulder;

        if (boulderIsSmall)
        {
            boulder = Instantiate(smallBoulderPrefab, boulderSpawnPosition, Quaternion.identity);
        }
        else
        {
            boulder = Instantiate(bigBoulderPrefab, boulderSpawnPosition, Quaternion.identity);
        }

        foreach (var levelSideCollider in GetComponents<Collider2D>())
        {
            // Ignore boulder collision with the level left and right side colliders
            Physics2D.IgnoreCollision(boulder.GetComponent<Collider2D>(), levelSideCollider);
        }

        nextBoulderSpawnTime = Time.time + Random.Range(boulderSpawnIntervalMin, boulderSpawnIntervalMax);
    }

    /**
     * @brief Spawns the ground collider and graphics
     */
    private void SpawnGround()
    {
        var groundColliderGameObject = new GameObject("GroundCollider");
        var groundCollider = groundColliderGameObject.AddComponent<BoxCollider2D>();
        groundCollider.size = new Vector2(40, 4);
        groundCollider.GetComponent<Transform>().Rotate(0, 0, slopeAngleDegrees);
        // Put the player a bit (0.1f) inside the ground graphics, so it looks a bit better
        groundCollider.GetComponent<Transform>().position = new Vector3(0, groundOffsetY - 0.1f);

        float groundImageWidth = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        for (float groundX = 0.0f, groundY = groundOffsetY;
            groundX < levelTotalLength;
            groundX += groundImageWidth * Mathf.Cos(slopeAngleRadians),
            groundY += groundImageWidth * Mathf.Sin(slopeAngleRadians))
        {
            var ground = Instantiate(groundPrefab,
                                     new Vector3(groundX, groundY, -5.0f),
                                     Quaternion.identity);
            ground.GetComponent<Transform>().Rotate(0, 0, slopeAngleDegrees);
            ground.GetComponent<Rigidbody2D>().velocity = new Vector2(-runningSpeed * Mathf.Cos(slopeAngleRadians),
                                                                      -runningSpeed * Mathf.Sin(slopeAngleRadians));
            nextLevelColliderPosition = new Vector3(groundX, groundY, 0.0f);
        }
    }

    /**
     * @brief Spawns the background graphics
     */
    private void SpawnBackground()
    {
        float backgroundImageWidth = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        for (float backgroundX = 0.0f;
             backgroundX < levelTotalLength;
             backgroundX += backgroundImageWidth)
        {
            var background = Instantiate(backgroundPrefab, new Vector3(backgroundX, -groundOffsetY, 100), Quaternion.identity);
            var backgroundRigidbody = background.GetComponent<Rigidbody2D>();
            backgroundRigidbody.velocity = new Vector2(-runningSpeed * Mathf.Cos(slopeAngleRadians) * 0.2f,
                                                       -runningSpeed * Mathf.Sin(slopeAngleRadians) * 0.2f);
        }
    }

    /**
     * @brief Spawns the next level collider and graphics
     */
    private void SpawnNextLevelCollider()
    {
        var nextLevelGameObject = new GameObject("NextLevelCollider");

        var nextLevelCollider = nextLevelGameObject.AddComponent<BoxCollider2D>();
        nextLevelCollider.isTrigger = true;
        nextLevelCollider.size = new Vector2(4, 20);
        nextLevelCollider.GetComponent<Transform>().position = nextLevelColliderPosition;

        var nextLevelRigidbody = nextLevelGameObject.AddComponent<Rigidbody2D>();
        nextLevelRigidbody.isKinematic = true;
        nextLevelRigidbody.velocity = new Vector2(-runningSpeed * Mathf.Cos(slopeAngleRadians),
                                                  -runningSpeed * Mathf.Sin(slopeAngleRadians));
    }
}
