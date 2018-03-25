using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelGenerator3 : MonoBehaviour {

    public Transform smallBoulderPrefab;
    public Transform bigBoulderPrefab;

    public Transform backgroundPrefab;
    public Transform groundPrefab;

    public float slopeAngleDegrees = -10.0f;

    private const float boulderSpawnIntervalMin = 3.0f; // As a function of difficulty?
    private const float boulderSpawnIntervalMax = 5.0f; // As a function of difficulty?
    private float nextBoulderSpawnTime = 0.0f;

    private float levelStartTime = 0.0f;
    private const float levelDuration = 60.0f; // As a function of difficulty?
    private const float firstSpawnTimeOffset = 3.0f;
    private const float lastSpawnTimeOffset = 5.0f;
    private const float runningSpeed = 2.0f;

    void Start()
    {
        levelStartTime = Time.time;
        nextBoulderSpawnTime = Time.time + firstSpawnTimeOffset + Random.Range(boulderSpawnIntervalMin, boulderSpawnIntervalMax);

        SpawnGround();
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
        Vector2 boulderSpawnPosition = new Vector2(-11.0f, 0.5f);

        // Probability that the spawned boulder is small
        const float probabilitySmall = 0.5f;

        // It must be between 0 and 1
        Assert.IsTrue(probabilitySmall >= 0.0f && probabilitySmall <= 1.0f);

        bool boulderIsSmall = (Random.Range(0.0f, 1.0f) <= probabilitySmall ? true : false);

        if (boulderIsSmall)
        {
            Instantiate(smallBoulderPrefab, boulderSpawnPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(bigBoulderPrefab, boulderSpawnPosition, Quaternion.identity);
        }

        nextBoulderSpawnTime = Time.time + Random.Range(boulderSpawnIntervalMin, boulderSpawnIntervalMax);
    }

    /**
     * @brief Spawns the ground collider and graphics
     */
    private void SpawnGround()
    {
        float slopeAngleRadians = slopeAngleDegrees / 360.0f * 2.0f * Mathf.PI;
        const float groundYOffset = -4.5f;

        var groundColliderGameObject = new GameObject("GroundCollider");
        var groundCollider = groundColliderGameObject.AddComponent<BoxCollider2D>();
        groundCollider.size = new Vector2(40, 4);
        groundCollider.GetComponent<Transform>().Rotate(0, 0, slopeAngleDegrees);
        groundCollider.GetComponent<Transform>().position = new Vector3(0, groundYOffset);

        float groundImageWidth = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        //float groundImageHeight = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        float levelTotalLength = runningSpeed * (levelDuration + firstSpawnTimeOffset + lastSpawnTimeOffset);
        Vector3 nextLevelColliderPosition = new Vector3(0, 0, 0);

        for (float groundX = 0.0f, groundY = groundYOffset;
            groundX > -levelTotalLength;
            groundX -= groundImageWidth * Mathf.Cos(slopeAngleRadians),
            groundY -= groundImageWidth * Mathf.Sin(slopeAngleRadians))
        {
            var ground = Instantiate(groundPrefab,
                                     new Vector3(groundX, groundY, 0.0f),
                                     Quaternion.identity);
            ground.GetComponent<Transform>().Rotate(0, 0, slopeAngleDegrees);
            ground.GetComponent<Rigidbody2D>().velocity = new Vector2(runningSpeed * Mathf.Cos(slopeAngleRadians),
                                                                      runningSpeed * Mathf.Sin(slopeAngleRadians));
            nextLevelColliderPosition = new Vector3(groundX, groundY, 0.0f);
        }

        var nextLevelGameObject = new GameObject("NextLevelCollider");
        var nextLevelCollider = nextLevelGameObject.AddComponent<BoxCollider2D>();
        var nextLevelRigidbody = nextLevelGameObject.AddComponent<Rigidbody2D>();
        nextLevelCollider.isTrigger = true;
        nextLevelCollider.size = new Vector2(4, 20);
        nextLevelCollider.GetComponent<Transform>().position = nextLevelColliderPosition;
        nextLevelRigidbody.isKinematic = true;
        nextLevelRigidbody.velocity = new Vector2(runningSpeed * Mathf.Cos(slopeAngleRadians),
                                                  runningSpeed * Mathf.Sin(slopeAngleRadians));
    }
}
