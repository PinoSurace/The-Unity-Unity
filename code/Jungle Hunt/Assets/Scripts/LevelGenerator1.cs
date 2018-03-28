using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Level generator script for level 1. The script generates ropes,
 * background, leaves and grass.
 */
public class LevelGenerator1 : MonoBehaviour {

    public Transform ropePrefab;

    public Transform backgroundPrefab;
    public Transform grassPrefab;
    public Transform leavesPrefab;

    public Transform waterPrefab;

    private float lastRopeX = 0.0f;
    private float levelEndLength = 0.0f;

    // Don't touch these two values. They are hard-coded to fit the existing rope prefab.
    private const float ropeJointXOffset = 1.7f;
    private const float ropeJointYOffset = 1.0f;

    private int difficulty;

    // Y coordinate of the ropes
    const float ropeY = 4.0f; // Should this be constant or also vary between some limits?

    // These following three values together define the length of level 1
    // Minimum distance between ropes
    private int ropeMinDistance;

    // Maximum distance between ropes
    private int ropeMaxDistance;

    // Number of ropes generated
    private int numberOfRopes;

    // Minimum speed for ropes
    private const int ropeMinSpeed = 1;

    // Maximum speed for ropes
    private const int ropeMaxSpeed = 5;

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

        ropeMinDistance = 4 + difficulty;
        ropeMaxDistance = 5 + difficulty;
        numberOfRopes = 5 + 5 * difficulty;

        levelEndLength = waterPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        GenerateRopes();
        GenerateBackground();
        GenerateFoliage();
        GenerateLevelEnd();
    }

    /**
     * @brief Generates ropes having a random distance between each other.
     */
    void GenerateRopes()
    {
        lastRopeX = ropeJointXOffset;
        var ropeDistance = 0;
        for (int n = 0; n < numberOfRopes; n++)
        {
            var rope = Instantiate(ropePrefab, new Vector3(lastRopeX - ropeDistance, ropeY, 97), Quaternion.identity);

            // We also need to move the connected anchor of the first element's hinge joint to the correct place
            var firstElement = rope.Find("rope_1");
            firstElement.gameObject.GetComponent<HingeJoint2D>().connectedAnchor
                = new Vector2(lastRopeX - ropeDistance - ropeJointXOffset, ropeY + ropeJointYOffset);

            // Set a random speed (gravity scale) for each rope
            var ropeSpeed = Random.Range(ropeMinSpeed, ropeMaxSpeed);
            for (int i = 0; i < rope.childCount; i++)
            {
                var ropeElement = rope.GetChild(i);
                ropeElement.gameObject.GetComponent<Rigidbody2D>().gravityScale = ropeSpeed;
            }

            lastRopeX -= ropeDistance;
            ropeDistance = Random.Range(ropeMinDistance, ropeMaxDistance);
        }
    }

    /**
     * @brief Generates the background for the whole duration of the level 
     */
    void GenerateBackground()
    {
        float backgroundImageWidth = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        for (float x = 0; x > lastRopeX - levelEndLength - backgroundImageWidth; x -= backgroundImageWidth)
        {
            Instantiate(backgroundPrefab, new Vector3(x, 0, 100), Quaternion.identity);
        }
    }

    /**
     * @brief Generates the grass and leaves for the whole duration of the level 
     */
    void GenerateFoliage()
    {
        float leavesImageWidth = leavesPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        for (float x = 0; x > lastRopeX - levelEndLength - leavesImageWidth; x -= leavesImageWidth)
        {
            Instantiate(leavesPrefab, new Vector3(x, 4.5f, 94), Quaternion.identity);
        }

        // Grass must be generated from the end towards the start, because we need to place the
        // water to start from an exact place at the end of the scene and not be defined by a
        // multiple of the grass image width
        float grassImageWidth = grassPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float grassStartX = lastRopeX + grassImageWidth / 2.0f - ropeJointXOffset;
        for (float x = grassStartX; x < grassImageWidth; x += grassImageWidth)
        {
            Instantiate(grassPrefab, new Vector3(x, -3.5f, -2), Quaternion.identity);
        }
    }

    /**
     * @brief Adds water and collision boxes for diving animation and changing to the next level
     */
    void GenerateLevelEnd()
    {
        Instantiate(waterPrefab, new Vector3(lastRopeX - levelEndLength/2.0f - ropeJointXOffset, -4.0f, 94), Quaternion.identity);

        GameObject gameObject = new GameObject("DivingAnimationCollider");
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(levelEndLength, 10.0f);
        collider.isTrigger = true;
        collider.transform.position = new Vector3(lastRopeX - levelEndLength / 2.0f - ropeJointXOffset, 0, 0);

        gameObject = new GameObject("NextLevelCollider");
        collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(levelEndLength, 1.0f);
        collider.isTrigger = true;
        collider.transform.position = new Vector3(lastRopeX - levelEndLength / 2.0f - ropeJointXOffset, -5.5f, 0);
    }
}