using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelGenerator4 : MonoBehaviour {

    public Transform cannibalPrefab;

    public Transform levelPiecePrefab;
    public Transform EndPrefab;

    private float lastPositionX = 0.0f;
    private float levelEndLength = 0.0f;

    private int difficulty;

    // Y coordinate of the cannibals
    const float cannibalY = -3.30f;

    // These following three values together define the length of level 1
    // Minimum distance between ropes
    private float cannibalSpaceInBetweenMin;

    // Maximum distance between ropes
    private float cannibalSpaceInBetweenMax;

    // Number of ropes generated
    private int numberOfCannibals;

    // Minimum speed for ropes
    private const float cannibalMinSpeed = 0.50f;

    // Maximum speed for ropes
    private const float cannibalMaxSpeed = 1.50f;

    void Start()
    {
        try
        {
            difficulty = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>().GetDifficulty();
        }
        catch
        {
            difficulty = 100;
        }

        cannibalSpaceInBetweenMin = 2.50f - (difficulty / 200);
        cannibalSpaceInBetweenMax = 3.20f - (difficulty / 200);
        numberOfCannibals = 2 + (difficulty / 20);

        levelEndLength = EndPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        GenerateEnemies();
        GenerateGround();
        GenerateLevelEnd();
    }

    /**
     * @brief Generates ropes having a random distance between each other.
     */
    void GenerateEnemies()
    {
        lastPositionX = cannibalSpaceInBetweenMax;
        var enemyDistance = cannibalSpaceInBetweenMax + 0.50f;
        for (int n = 0; n < numberOfCannibals; n++)
        {
            var cannibalPatrolDist = Random.Range(cannibalSpaceInBetweenMin, cannibalSpaceInBetweenMax);
            enemyDistance += cannibalPatrolDist;
            var enemy = Instantiate(cannibalPrefab, new Vector3(lastPositionX + enemyDistance, cannibalY, 97), Quaternion.identity);

            // Set a random speed (gravity scale) for each rope
            var cannibalSpeed = Random.Range(cannibalMinSpeed, cannibalMaxSpeed);
            enemy.GetComponentInChildren<Cannibal_Script>().speed = cannibalSpeed;

            enemy.GetComponentInChildren<Cannibal_Script>().minDist = - cannibalPatrolDist;
            enemy.GetComponentInChildren<Cannibal_Script>().maxDist = cannibalPatrolDist;

            lastPositionX += enemyDistance;
            enemyDistance = cannibalPatrolDist;
        }
    }

    /**
     * @brief Generates the background for the whole duration of the level 
     */
    void GenerateGround()
    {
        float backgroundImageWidth = levelPiecePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        for (float x = 0; x < lastPositionX + backgroundImageWidth; x += backgroundImageWidth)
        {
            Instantiate(levelPiecePrefab, new Vector3(x, 0, 1), Quaternion.identity);
        }
        this.GetComponent<BoxCollider2D>().size = new Vector2(lastPositionX * 2, 0.40f);
        this.GetComponent<BoxCollider2D>().offset = new Vector2(lastPositionX / 2, -4.22f);
    }

    /**
     * @brief Adds water and collision boxes for diving animation and changing to the next level
     */
    void GenerateLevelEnd()
    {
        Instantiate(EndPrefab, new Vector3(lastPositionX + levelEndLength / 2.0f, 0f, 1), Quaternion.identity);
    }
}
